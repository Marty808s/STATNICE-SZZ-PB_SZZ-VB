import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Nav from "@core/Nav";
import DropDown from "@form/DropDown";
import TextField from "@form/TextField";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAPI } from "@api/api";
import { useMessage } from "@hooks/MessageContext";

export default function ObjednavkaFormPage() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const { addMessage } = useMessage();
    const { 
        getZakaznici, 
        getProdukty, 
        getObjednavkaDetail,
        createObjednavka, 
        addProduktyDoObjednavky,
        updateObjednavka,
        updateProduktyObjednavky
    } = useAPI();

    const action = (searchParams.get('action') || '').toLowerCase();
    const orderId = searchParams.get('id');
    const isEdit = action === 'edit' && orderId;
    const isCreate = action === 'create';

    const [customers, setCustomers] = useState([]);
    const [products, setProducts] = useState([]);
    const [form, setForm] = useState({ customer_id: "", currency: "CZK" });
    const [items, setItems] = useState([{ product_id: "", quantity: "", unit_price: "" }]);
    const [loading, setLoading] = useState(false);

    const currencies = useMemo(() => ([
        { label: 'CZK', value: 'CZK' },
        { label: 'EUR', value: 'EUR' }
    ]), []);

    const loadData = useCallback(async () => {
        try {
            const [cust, prods] = await Promise.all([getZakaznici(), getProdukty()]);
            setCustomers(cust || []);
            setProducts(prods || []);
        } catch (e) {
            addMessage('Nepodařilo se načíst data', 'E');
        }
    }, [getZakaznici, getProdukty, addMessage]);

    const loadOrderData = useCallback(async () => {
        if (!isEdit) return;
        try {
            setLoading(true);
            const orderData = await getObjednavkaDetail(orderId);
            const { order, products: orderProducts } = orderData;
            
            setForm({
                customer_id: String(order.customer_id || ""),
                currency: order.currency || "CZK"
            });
            
            if (orderProducts && orderProducts.length > 0) {
                setItems(orderProducts.map(p => ({
                    product_id: String(p.product_id),
                    quantity: String(p.quantity),
                    unit_price: String(p.unit_price)
                })));
            } else {
                setItems([{ product_id: "", quantity: "", unit_price: "" }]);
            }
        } catch (err) {
            addMessage('Nepodařilo se načíst objednávku', 'E');
        } finally {
            setLoading(false);
        }
    }, [isEdit, orderId, getObjednavkaDetail, addMessage]);

    useEffect(() => { 
        loadData(); 
    }, []);

    useEffect(() => {
        loadOrderData();
    }, []);

    const handleForm = (patch) => setForm(prev => ({ ...prev, ...patch }));

    const handleItemChange = (index, patch) => {
        setItems(prev => prev.map((it, i) => i === index ? ({ ...it, ...patch }) : it));
    };

    const addItemRow = () => setItems(prev => ([...prev, { product_id: "", quantity: "", unit_price: "" }]));
    const removeItemRow = (index) => setItems(prev => prev.filter((_, i) => i !== index));

    const productOptions = useMemo(() => (products || []).map(p => ({ 
        label: `${p.name} (ID ${p.id})`, 
        value: String(p.id) 
    })), [products]);
    
    const customerOptions = useMemo(() => (customers || []).map(c => ({ 
        label: `${c.name} ${c.surname} (${c.email})`, 
        value: String(c.id) 
    })), [customers]);

    const handleSubmit = useCallback(async (e) => {
        e.preventDefault();
        try {
            if (!form.customer_id) {
                addMessage('Vyberte zákazníka', 'E');
                return;
            }
            
            const cleanItems = items
                .map(it => ({ 
                    product_id: Number(it.product_id), 
                    quantity: Number(it.quantity), 
                    unit_price: Number(it.unit_price) 
                }))
                .filter(it => it.product_id && it.quantity > 0 && !Number.isNaN(it.unit_price));

            if (cleanItems.length === 0) {
                addMessage('Přidejte alespoň jednu položku', 'E');
                return;
            }

            if (isEdit) {
                // Update existing order
                await updateObjednavka(orderId, { 
                    customer_id: Number(form.customer_id), 
                    currency: form.currency 
                });
                await updateProduktyObjednavky(orderId, cleanItems);
                addMessage('Objednávka byla upravena', 'S');
            } else {
                // Create new order
                const created = await createObjednavka({ 
                    customer_id: Number(form.customer_id), 
                    currency: form.currency 
                });
                await addProduktyDoObjednavky(created.id, cleanItems);
                addMessage('Objednávka byla vytvořena', 'S');
                navigate(`/objednavky/${created.id}`);
                return;
            }
            
            // For edit mode, go back to order list
            navigate('/objednavky');
        } catch (err) {
            addMessage(isEdit ? 'Úprava objednávky selhala' : 'Vytvoření objednávky selhalo', 'E');
        }
    }, [form, items, isEdit, orderId, createObjednavka, addProduktyDoObjednavky, updateObjednavka, updateProduktyObjednavky, navigate, addMessage]);

    const handleBack = () => navigate(-1);

    if (loading) {
        return (
            <>
                <Nav />
                <Container property="flex w-full justify-center m-4">
                    <Paragraph>Načítání...</Paragraph>
                </Container>
            </>
        );
    }

    return (
        <>
            <Nav />
            <Container property="flex w-full justify-center m-4">
                <Headings>{isEdit ? 'Úprava objednávky' : 'Vytvoření objednávky'}</Headings>
            </Container>

            <Container property="max-w-3xl mx-auto px-4">
                <Container property="bg-white border rounded-lg shadow-sm p-6">
                    <form onSubmit={handleSubmit}>
                        <Container property="flex flex-col gap-6">
                            <Container property="grid grid-cols-1 md:grid-cols-2 gap-4">
                                <DropDown
                                    id="customer_id"
                                    label="Zákazník"
                                    placeholder="Vyberte zákazníka"
                                    options={customerOptions}
                                    value={form.customer_id}
                                    onChange={handleForm}
                                    disableDefault={true}
                                    required={true}
                                />
                                <DropDown
                                    id="currency"
                                    label="Měna"
                                    placeholder="Vyberte měnu"
                                    options={currencies}
                                    value={form.currency}
                                    onChange={handleForm}
                                    disableDefault={true}
                                />
                            </Container>

                            <Container>
                                <Headings property="text-lg">Položky objednávky</Headings>
                                {items.map((it, idx) => (
                                    <Container key={idx} property="grid grid-cols-1 md:grid-cols-12 gap-3 items-end mt-3">
                                        <Container property="md:col-span-6">
                                            <DropDown
                                                id="product_id"
                                                label="Produkt"
                                                placeholder="Vyberte produkt"
                                                options={productOptions}
                                                value={it.product_id}
                                                onChange={(patch) => handleItemChange(idx, patch)}
                                                disableDefault={true}
                                                required={true}
                                            />
                                        </Container>
                                        <Container property="md:col-span-2">
                                            <TextField
                                                id="quantity"
                                                label="Množství"
                                                type="number"
                                                min="1"
                                                value={it.quantity}
                                                onChange={(patch) => handleItemChange(idx, patch)}
                                                placeholder="0"
                                            />
                                        </Container>
                                        <Container property="md:col-span-3">
                                            <TextField
                                                id="unit_price"
                                                label="Jednotková cena"
                                                type="number"
                                                step="0.01"
                                                min="0"
                                                value={it.unit_price}
                                                onChange={(patch) => handleItemChange(idx, patch)}
                                                placeholder="0.00"
                                            />
                                        </Container>
                                        <Container property="md:col-span-1">
                                            <Button type="button" icon="trash" onClick={() => removeItemRow(idx)}></Button>
                                        </Container>
                                    </Container>
                                ))}

                                <Container property="mt-3">
                                    <Button type="button" icon="plus" onClick={addItemRow}>Přidat položku</Button>
                                </Container>
                            </Container>

                            <Container property="flex gap-3 mt-4">
                                <Button type="button" icon="arrow" onClick={handleBack}>Zpět</Button>
                                <Button type="submit" icon="save">
                                    {isEdit ? 'Uložit změny' : 'Vytvořit objednávku'}
                                </Button>
                            </Container>
                        </Container>
                    </form>
                </Container>
            </Container>
        </>
    );
}


