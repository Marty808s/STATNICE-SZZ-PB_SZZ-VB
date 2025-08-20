import React from "react";
import ContainerForEntity from "./ContainerForEntity";
import Container from "./Container";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Paragraph from "@core/Text/Paragraph";

export default function PopUpCon({
    children, 
    onClose, 
    title, 
    text, 
    onSubmit, 
    onReject, 
    onSubmitText = "Ano", 
    onRejectText = "Ne", 
    variant = "gray",
    useCustomContainer = false,
    customContainer = null,
    isEditMode = false,
    onSave = null,
    onCancel = null
}) {
    const closePopUp = () => {
        if (onClose) {
            onClose();
        }
    };

    // Edit mode variant pro editaci produktu
    if (isEditMode) {
        return (
            <Container property={"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"}>
                <ContainerForEntity property={"w-1/2 max-w-2xl"} variant={variant}>
                    <Container property={"flex justify-between items-center mb-4"}>
                        <Headings property={"ml-4"} sizeTag={"h5"}>{title}</Headings>
                        <Button variant={"red"} onClick={closePopUp} property={"px-2 py-1"}>
                            X
                        </Button>
                    </Container>

                    {/* CUSTOM CHILDREN - form fields */}
                    <Container property={"mb-6 ml-4 mr-4"}>
                        {children}
                    </Container>

                    {/* TLAČÍTKA PRO EDITACI */}
                    <Container property={"flex justify-between ml-4 mr-4 mb-4 gap-4"}>
                        <Button 
                            property={"w-full"} 
                            onClick={onCancel || closePopUp}
                            variant="red"
                        >
                            Zrušit
                        </Button>
                        <Button 
                            property={"w-full"} 
                            onClick={onSave || closePopUp}
                            variant="green"
                        >
                            Uložit
                        </Button>
                    </Container>
                </ContainerForEntity>
            </Container>
        );
    }

    // Standardní variant
    return(
        <Container property={"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"}>
            <ContainerForEntity property={"w-1/2 max-w-2xl"} variant={variant}>
                <Container property={"flex justify-between items-center mb-4"}>
                    <Headings property={"ml-4"} sizeTag={"h5"}>{title}</Headings>
                    <Button variant={"red"} onClick={onClose} property={"px-2 py-1"}>
                        X
                    </Button>
                </Container>

                {/* TEXT BOXU */}
                <Container property={"mb-16 mt-16"}>
                    <Paragraph property={"text-center"}>{text ? text : "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."}</Paragraph>
                </Container>

                {/* BUTTONY */}
                <Container property={"flex justify-between ml-4 mr-4 gap-4"}>
                    <Button property={"w-full"} onClick={onSubmit}>{onSubmitText}</Button>
                    <Button variant={"red"} property={"w-full"} onClick={onReject}>{onRejectText}</Button>
                </Container>

                <Container property={"max-h-[80vh] overflow-y-auto"}>
                    {children}
                </Container>
            </ContainerForEntity>
        </Container>
    )
}