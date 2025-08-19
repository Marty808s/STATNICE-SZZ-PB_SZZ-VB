import React from 'react'
import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import ContainerForEntity from "@core/Container/ContainerForEntity";

export default function ProductEntity({entity, handleEdit}) {

    const renderStav = (stock) => {
        if (stock) {
            return "Skladem"
        }
        if (!stock) {
            return "Není skladem"
        }
    }

    return (
        <ContainerForEntity property="flex py-2 cursor-pointer mt-2" onClick={() => console.log("Klik na:", entity.id)}>
            <Container property="grid grid-cols-3 gap-8">
                <Paragraph>{entity.name}</Paragraph>
                <Paragraph>Skladem: {entity.in_stock} {entity.piece_unit_type}</Paragraph>
                <Paragraph variant={entity.in_stock ? "green" : "red"}>{renderStav(entity.stock)}</Paragraph>
            </Container>

            <Container property="flex flex-cols justify-end ml-auto gap-2">
                <Button icon="edit" onClick={(e) => handleEdit(e, entity)}></Button>
                <Button icon="doc" onClick={(e) => handleEdit(e, entity)}></Button>
            </Container>

        </ContainerForEntity>
    )
}
