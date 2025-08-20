import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Nav from "@core/Nav";
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAPI } from "@api/api";
import OrderProductEntity from "@components/Entity/OrderProductEntity";
import PopUpCon from "@core/Container/PopUpCon";
import EditProductForm from "@components/Entity/EditProductForm";

export default function ObjednavkaDetailPage() {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { id } = useParams();
    const navigate = useNavigate();
    const { getObjednavkaDetail } = useAPI();
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [showEditPopup, setShowEditPopup] = useState(false);

    const initFetch = async () => {
        try {
            setLoading(true);
            const res = await getObjednavkaDetail(id);
            setData(res);
            setError(null);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        if (id) {
            initFetch();
        }
    }, [id])

    useEffect(() => {
        console.log("Vybraný produkt", selectedProduct);
    }, [selectedProduct])

    const handleBack = () => {
        navigate('/objednavky');
    }

    const handleEdit = (e, product) => {
        e.stopPropagation();
        setSelectedProduct(product);
        setShowEditPopup(true);
        console.log("Kliknutí na produkt:", product.id, "objednávky:", id);
    }

    const handleSaveProduct = (updatedProduct) => {
        console.log("Uložení produktu:", updatedProduct);
        // Zde byste implementovali volání API pro uložení změn
        setShowEditPopup(false);
        setSelectedProduct(null);
        // Možná refresh dat
        // initFetch();
    }

    const handleCancelEdit = () => {
        setShowEditPopup(false);
        setSelectedProduct(null);
    }

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

    if (error) {
        return (
            <>
                <Nav />
                <Container property="flex w-full justify-center m-4">
                    <Paragraph property="text-red-600">Chyba: {error}</Paragraph>
                </Container>
            </>
        );
    }

    if (!data) {
        return (
            <>
                <Nav />
                <Container property="flex w-full justify-center m-4">
                    <Paragraph>Objednávka nenalezena</Paragraph>
                </Container>
            </>
        );
    }

    const { order, products } = data;

    return (
        <>
            <Nav />
            <Container property="flex w-full justify-center m-4">
                <Headings>Detail objednávky #{order.id}</Headings>
            </Container>

            {/* Informace o objednávce */}
            <Container property="max-w-4xl mx-auto px-4 mb-6">
                <Container property="bg-white border rounded-lg shadow-sm p-6">
                    <Container property="grid grid-cols-2 gap-6">
                        <Container>
                            <Paragraph property="text-sm text-gray-500 mb-1">Zákazník</Paragraph>
                            <Paragraph property="text-lg font-semibold">{order.name} {order.surname}</Paragraph>
                        </Container>
                        <Container>
                            <Paragraph property="text-sm text-gray-500 mb-1">Email</Paragraph>
                            <Paragraph property="text-lg">{order.email}</Paragraph>
                        </Container>
                        <Container>
                            <Paragraph property="text-sm text-gray-500 mb-1">Stav objednávky</Paragraph>
                            <Container property={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${
                                order.state === "Zpracování" ? "bg-red-100 text-red-800" :
                                order.state === "Export" ? "bg-yellow-100 text-yellow-800" :
                                "bg-green-100 text-green-800"
                            }`}>
                                {order.state}
                            </Container>
                        </Container>
                        <Container>
                            <Paragraph property="text-sm text-gray-500 mb-1">Celková cena</Paragraph>
                            <Paragraph property="text-2xl font-bold text-green-600">{order.total_price} {order.currency}</Paragraph>
                        </Container>
                    </Container>
                </Container>
            </Container>

            {/* Seznam produktů */}
            <Container property="max-w-4xl mx-auto px-4">
                <Container property="flex justify-between items-center mb-4">
                    <Headings property="text-xl">Produkty v objednávce</Headings>
                    <Button icon="arrow" onClick={handleBack}>Zpět na objednávky</Button>
                </Container>
                
                {products.length === 0 ? (
                    <Container property="text-center py-8">
                        <Paragraph property="text-gray-500">Žádné produkty v objednávce</Paragraph>
                    </Container>
                ) : (
                    products.map((product) => (
                        <OrderProductEntity 
                            key={product.id} 
                            product={product} 
                            handleEdit={handleEdit}
                        />
                    ))
                )}
            </Container>

            {/* Popup pro editaci produktu */}
            {showEditPopup && selectedProduct && (
                <PopUpCon
                    title={`Editace produktu: ${selectedProduct.product_name}`}
                    onClose={handleCancelEdit}
                    isEditMode={true}
                    onSave={handleSaveProduct}
                    onCancel={handleCancelEdit}
                >
                    <EditProductForm 
                        product={selectedProduct}
                        onSave={handleSaveProduct}
                    />
                </PopUpCon>
            )}
        </>
    )
}
