import React from 'react'
import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import ContainerForEntity from "@core/Container/ContainerForEntity";

export default function ObjednavkaEntity({entity, handleEdit, handleViewDetail}) {

    const formatPrice = (price) => {
        return `${price}`;
    };

    const formatCustomerName = (name, surname) => {
        return `${name} ${surname}`;
    };

    const formatState = (state) => {
        if (state === "Zpracování") {
            return "bg-red-100 text-red-800"
        }
        if (state === "Export") {
            return "bg-yellow-100 text-yellow-800"
        }
        if (state === "Dokončená") {
            return "bg-green-100 text-green-800"
        }
        return "bg-gray-100 text-gray-800" // fallback
    }

    return (
        <ContainerForEntity property="flex py-4 cursor-pointer mt-3 hover:bg-gray-50 transition-colors border rounded-lg shadow-sm" onClick={(e) => handleViewDetail(e, entity)}>
            
            <Container property="flex items-center px-4 flex-1">
                
                <Container property="w-16 flex-shrink-0">
                    <Paragraph property="text-sm text-gray-500 mb-1">ID</Paragraph>
                    <Paragraph property="text-sm font-mono text-blue-600">#{entity.id}</Paragraph>
                </Container>
            
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Zákazník</Paragraph>
                    <Paragraph property="text-sm text-gray-800">{formatCustomerName(entity.name, entity.surname)}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Email</Paragraph>
                    <Paragraph property="text-sm text-gray-600 truncate">{entity.email}</Paragraph>
                </Container>
                
                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Cena</Paragraph>
                    <Paragraph property="text-sm text-green-600">{formatPrice(entity.total_price)} {entity.currency}</Paragraph>
                </Container>

                <Container property="flex flex-col flex-1 ml-6">
                    <Paragraph property="text-sm text-gray-500 mb-1">Stav</Paragraph>
                    <Container property={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${formatState(entity.state)}`}>
                        {entity.state}
                    </Container>
                </Container>

            </Container>

            <Container property="flex justify-end ml-6 gap-3 pr-4">
                <Button icon="doc" onClick={(e) => handleEdit(e, entity)} title="Upravit objednávku"></Button>
                {/*<Button icon="doc" onClick={(e) => handleViewDetail(e, entity)} title="Zobrazit detail"></Button>*/}
            </Container>

        </ContainerForEntity>
    )
}
