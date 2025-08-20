import React from 'react'
import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import ContainerForEntity from "@core/Container/ContainerForEntity";

export default function OrderProductEntity({product, handleEdit}) {

    const formatPrice = (price) => {
        return `${price} CZK`;
    };

    const formatTotal = (total) => {
        return `${total} CZK`;
    };

    return (
        <ContainerForEntity property="flex py-3 cursor-pointer mt-2 hover:bg-gray-50 transition-colors border rounded-lg shadow-sm" onClick={(e) => handleEdit(e, product)}>
            
            <Container property="flex items-center px-4 flex-1">
                
                <Container property="w-16 flex-shrink-0">
                    <Paragraph property="text-sm text-gray-500 mb-1">ID</Paragraph>
                    <Paragraph property="text-sm font-mono text-blue-600">#{product.product_id}</Paragraph>
                </Container>
            
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Název produktu</Paragraph>
                    <Paragraph property="font-medium text-gray-800">{product.product_name}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Množství</Paragraph>
                    <Paragraph property="text-sm text-gray-600">{product.quantity} {product.piece_unit_type}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Jednotková cena</Paragraph>
                    <Paragraph property="text-sm text-gray-600">{formatPrice(product.unit_price)}</Paragraph>
                </Container>

                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Celkem</Paragraph>
                    <Paragraph property="font-semibold text-green-600">{formatTotal(product.line_total)}</Paragraph>
                </Container>

            </Container>

        </ContainerForEntity>
    )
}
