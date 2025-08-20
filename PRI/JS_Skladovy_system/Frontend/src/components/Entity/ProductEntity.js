import React from 'react'
import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import ContainerForEntity from "@core/Container/ContainerForEntity";

export default function ProductEntity({entity, handleEdit}) {

    const formatStock = (stock) => {
        if (stock) {
            return "bg-green-100 text-green-800"
        }
        if (!stock) {
            return "bg-red-100 text-red-800"
        }
        return "bg-gray-100 text-gray-800" // fallback
    }

    const renderStav = (stock) => {
        if (stock) {
            return "Skladem"
        }
        if (!stock) {
            return "Není skladem"
        }
        return "Neznámý stav"
    }

    return (
        <ContainerForEntity property="flex py-4 cursor-pointer mt-3 hover:bg-gray-50 transition-colors border rounded-lg shadow-sm" onClick={() => console.log("Klik na:", entity.id)}>
            
            <Container property="flex items-center px-4 flex-1">
                
                <Container property="w-16 flex-shrink-0">
                    <Paragraph property="text-sm text-gray-500 mb-1">ID</Paragraph>
                    <Paragraph property="text-sm font-mono text-blue-600">#{entity.id}</Paragraph>
                </Container>
            
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Název produktu</Paragraph>
                    <Paragraph property="font-medium text-gray-800">{entity.name}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Skladem</Paragraph>
                    <Paragraph property="text-sm text-gray-600">{entity.in_stock} {entity.piece_unit_type}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Stav</Paragraph>
                    <Container property={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${formatStock(entity.stock)}`}>
                        {renderStav(entity.stock)}
                    </Container>
                </Container>

            </Container>

            <Container property="flex justify-end ml-6 gap-3 pr-4">
                <Button icon="edit" onClick={(e) => handleEdit(e, entity)} title="Upravit produkt"></Button>
                <Button icon="doc" onClick={(e) => handleEdit(e, entity)} title="Zobrazit detail"></Button>
            </Container>

        </ContainerForEntity>
    )
}
