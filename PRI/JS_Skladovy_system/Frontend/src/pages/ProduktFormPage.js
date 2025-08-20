import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Nav from "@core/Nav";
import TextField from "@form/TextField";
import DropDown from "@form/DropDown";
import TextBox from "@form/TextBox";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useMessage } from "@hooks/MessageContext";
import { useAPI } from "@api/api";

export default function ProduktFormPage() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const { addMessage } = useMessage();
    const { getProdukt, createProdukt, updateProdukt } = useAPI();

    const action = (searchParams.get('action') || '').toLowerCase();
    const productId = searchParams.get('id');

    const isEdit = action === 'edit' && productId;

    const [formData, setFormData] = useState({
        name: "",
        piece_unit_type: "",
        in_stock: 0,
        description: "",
        unit_price: ""
    });

    const unitOptions = useMemo(() => ([
        { label: "kusy (ks)", value: "ks" },
        { label: "metry (m)", value: "m" },
        { label: "kilogramy (kg)", value: "kg" }
    ]), []);

    const handleFieldChange = (patch) => {
        setFormData((prev) => ({ ...prev, ...patch }));
    };

    const initFetch = useCallback(async () => {
        if (!isEdit) return;
        try {
            const data = await getProdukt(productId);
            setFormData({
                name: data.name || "",
                piece_unit_type: data.piece_unit_type || "",
                in_stock: Number(data.in_stock ?? 0),
                description: data.description || "",
                unit_price: String(data.unit_price ?? "")
            });
        } catch (err) {
            addMessage("Nepodařilo se načíst produkt", "E");
        }
    }, [isEdit, productId, getProdukt, addMessage]);

    useEffect(() => {
        initFetch();
    }, [productId]);

    const handleSubmit = useCallback(async (e) => {
        e.preventDefault();
        try {
            const payload = {
                name: formData.name?.trim(),
                piece_unit_type: formData.piece_unit_type,
                in_stock: Number(formData.in_stock) || 0,
                description: formData.description || null,
                unit_price: Number(formData.unit_price)
            };

            if (!payload.name || !payload.piece_unit_type || Number.isNaN(payload.unit_price)) {
                addMessage("Vyplňte prosím povinná pole", "E");
                return;
            }

            if (isEdit) {
                await updateProdukt(productId, payload);
                addMessage("Produkt byl upraven", "S");
            } else {
                await createProdukt(payload);
                addMessage("Produkt byl vytvořen", "S");
            }
            navigate('/produkty');
        } catch (err) {
            addMessage("Uložení se nezdařilo", "E");
        }
    }, [formData, isEdit, productId, createProdukt, updateProdukt, addMessage, navigate]);

    const handleBack = () => navigate(-1);

    return (
        <>
            <Nav />
            <Container property="flex w-full justify-center m-4">
                <Headings>{isEdit ? "Úprava produktu" : "Vytvoření produktu"}</Headings>
            </Container>

            <Container property="max-w-xl mx-auto px-4">
                <Container property="bg-white border rounded-lg shadow-sm p-6">
                    <form onSubmit={handleSubmit}>
                        <Container property="flex flex-col gap-4">
                            <TextField
                                id="name"
                                label="Název produktu"
                                placeholder="Zadejte název"
                                required={true}
                                value={formData.name}
                                onChange={handleFieldChange}
                            />

                            <DropDown
                                id="piece_unit_type"
                                label="Jednotka"
                                placeholder="Vyberte jednotku"
                                options={unitOptions}
                                required={true}
                                value={formData.piece_unit_type}
                                onChange={handleFieldChange}
                                disableDefault={true}
                            />

                            <TextField
                                id="in_stock"
                                label="Množství skladem"
                                type="number"
                                placeholder="0"
                                value={formData.in_stock}
                                onChange={handleFieldChange}
                            />

                            <TextBox
                                id="description"
                                label="Popis"
                                rows={6}
                                value={formData.description}
                                onChange={handleFieldChange}
                            />

                            <TextField
                                id="unit_price"
                                label="Jednotková cena"
                                type="number"
                                placeholder="0.00"
                                value={formData.unit_price}
                                onChange={handleFieldChange}
                            />

                            <Container property="flex items-center gap-2 mt-2">
                                <Paragraph property="text-sm text-gray-500">Stav:</Paragraph>
                                <Container property={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${Number(formData.in_stock) > 0 ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800"}`}>
                                    {Number(formData.in_stock) > 0 ? "Skladem" : "Není skladem"}
                                </Container>
                            </Container>

                            <Container property="flex gap-3 mt-4">
                                <Button type="button" variant="blue" onClick={handleBack}>Zpět</Button>
                                <Button type="submit" variant="green" icon="save">Uložit</Button>
                            </Container>
                        </Container>
                    </form>
                </Container>
            </Container>
        </>
    );
}


