import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Nav from "@core/Nav";
import TextField from "@form/TextField";
import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUser } from "@hooks/UserProvider";
import { useMessage } from "@hooks/MessageContext";

export default function IndexPage() {
    const [formData, setFormData] = useState({ username: "", password: "" })
    const { user, setUser } = useUser()
    const navigate = useNavigate();
    const { addMessage } = useMessage();

    const handleFieldChange = (patch) => {
        setFormData((prev) => ({ ...prev, ...patch }))
    }

    // debug
    useEffect(() => {
        console.log(formData);
    }, [formData])

    const handleSubmit = useCallback(async (e) => {
        e.preventDefault()
        try {
            const res = await fetch("http://localhost:8081/api/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ username: formData.username, password: formData.password })
            })

            if (!res.ok) {
                addMessage("Špatné přihlašovací údaje", "E")
                return
            }

            const data = await res.json()
            console.log("Login response:", data)

            if (!data || !data.id || !data.role || !data.username) {
                addMessage("Neplatná odpověď serveru", "E")
                return
            }

            setUser(data)
            navigate('/produkty')
            addMessage("Uživatel byl úspěšně přihlášen!", "S")

        } catch (err) {
            console.error("Login error:", err)
            addMessage("Přihlášení neproběhlo úspěšně!", "E")

        }
    }, [formData, addMessage, navigate, setUser])

    return (
        <>
            <Nav/>
            <Container property="flex w-full justify-center m-4">
                <Container property="w-64 mt-20 rounded shadow-md border border-gray-300">
                    <Container property="flex gap-4 justify-center p-4">

                        <form onSubmit={handleSubmit}>
                            <Container property="flex flex-col gap-4">
                                <Headings property="text-center">Přihlaste se</Headings>

                                <TextField
                                    id="username"
                                    name="username"
                                    label="Uživatelské jméno"
                                    placeholder="Zadejte své uživatelské jméno"
                                    icon="user"
                                    required={true}
                                    value={formData.username}
                                    onChange={handleFieldChange}
                                />

                                <TextField
                                    id="password"
                                    name="password"
                                    type="password"
                                    label="Heslo"
                                    placeholder="Zadejte své heslo"
                                    icon="lock"
                                    required={true}
                                    value={formData.password}
                                    onChange={handleFieldChange}
                                />
                                    
                                <Button type="submit">
                                    Přihlaste se
                                </Button>

                            </Container>

                        </form>

                    </Container>
                    
                </Container>
            </Container>
        </>
    )
}