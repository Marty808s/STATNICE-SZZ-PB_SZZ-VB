import React, { useState, useEffect } from 'react';
import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import TextField from "@form/TextField";

export default function EditProductForm({ product, onSave }) {
    const [formData, setFormData] = useState({
        quantity: '',
        unit_price: ''
    });

    useEffect(() => {
        if (product) {
            setFormData({
                quantity: product.quantity?.toString() || '',
                unit_price: product.unit_price?.toString() || ''
            });
        }
    }, [product]);

    const handleChange = (patch) => {
        setFormData(prev => ({
            ...prev,
            ...patch
        }));
    };

    const handleSubmit = () => {
        if (onSave) {
            onSave({
                ...product,
                quantity: parseInt(formData.quantity) || 0,
                unit_price: parseFloat(formData.unit_price) || 0
            });
        }
    };

    return (
        <Container property="space-y-4">
            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">Název produktu</Paragraph>
                <Paragraph property="font-medium text-gray-800">{product?.product_name}</Paragraph>
            </Container>

            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">ID produktu</Paragraph>
                <Paragraph property="font-mono text-blue-600">#{product?.product_id}</Paragraph>
            </Container>

            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">Množství *</Paragraph>
                <TextField
                    id="quantity"
                    property="w-full"
                    type="number"
                    min="1"
                    value={formData.quantity}
                    onChange={handleChange}
                    placeholder="Zadejte množství"
                />
            </Container>

            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">Jednotková cena (CZK) *</Paragraph>
                <TextField
                    id="unit_price"
                    property="w-full"
                    type="number"
                    step="0.01"
                    min="0"
                    value={formData.unit_price}
                    onChange={handleChange}
                    placeholder="Zadejte cenu"
                />
            </Container>

            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">Jednotka</Paragraph>
                <Paragraph property="text-gray-800">{product?.piece_unit_type}</Paragraph>
            </Container>

            <Container>
                <Paragraph property="text-sm text-gray-500 mb-1">Aktuální celkem</Paragraph>
                <Paragraph property="font-semibold text-green-600">
                    {(parseFloat(formData.quantity) * parseFloat(formData.unit_price)).toFixed(2)} CZK
                </Paragraph>
            </Container>
        </Container>
    );
}
