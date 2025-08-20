import Container from "@core/Container/Container";
import Paragraph from "@core/Text/Paragraph";
import Button from "@core/Button/Button";
import Headings from "@core/Text/Headings";
import Nav from "@core/Nav";
import TextField from "@form/TextField";
import DropDown from "@form/DropDown";
import { useCallback, useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useUser } from "@hooks/UserProvider";
import { useMessage } from "@hooks/MessageContext";
import { useAPI } from "@api/api";
import ProductEntity from "@components/Entity/ProductEntity";

export default function ProduktyPage() {
    const [data, setData] = useState([]);
    const [filter, setFilter] = useState({ nazev: "", skladem: "" }); 
    const { getProdukty } = useAPI();
    const [searchParams, setSearchParams] = useSearchParams();
    const navigate = useNavigate();

    const handleChange = (patch) => {
        setFilter((prev) => ({ ...prev, ...patch }))
    }

    const initFetch = async() => {
        const res = await getProdukty();
        setData(res);
    }

    //filtrované hodnoty 
    const normalizedName = (filter.nazev || "").toLowerCase()
    const sklademFilter = filter.skladem === "" ? null : Number(filter.skladem)
    const filteredValues = Array.isArray(data)
        ? data.filter((p) => {
            const nameMatch = (p.name || "").toLowerCase().includes(normalizedName)
            const sklademMatch = sklademFilter === null ? true : Number(p.stock) === sklademFilter
            return nameMatch && sklademMatch
        })
        : []

    useEffect(() => {
        initFetch();
    }, [])

    // init načtení parametrů
    useEffect(() => {
        const nazevParam = searchParams.get('nazev') || "";
        const sklademParam = searchParams.get('skladem') || "";
        if (nazevParam !== "" || sklademParam !== "") {
            setFilter((prev) => ({ ...prev, nazev: nazevParam, skladem: sklademParam }));
        }
    }, [])

    // změna parametrů v url
    useEffect(() => {
        const params = {};
        if (filter.nazev) params.nazev = filter.nazev;
        if (filter.skladem !== "" && filter.skladem !== null && filter.skladem !== undefined) {
            params.skladem = String(filter.skladem);
        }
        setSearchParams(params, { replace: true });
    }, [filter, setSearchParams])

    // debug konzole
    useEffect(() => {
        console.log(data)
        console.log(filter)
    }, [data, filter])

    // handlery
    const handleEdit = (e, entity) => {
        e.stopPropagation();
        navigate(`/produkt?action=edit&id=${entity.id}`);
    }

    return (
    <>
        <Nav/>
        <Container property="flex w-full justify-center m-4">
            <Headings>Produkty</Headings>
        </Container>
        <Container property="w-full flex justify-center m-4 gap-2" >
            <TextField 
                id="nazev" 
                property="w-64" 
                placeholder="Hledejte podle názvu" 
                value={filter.nazev || ""}
                onChange={handleChange}
            />
            <DropDown
                id="skladem"
                placeholder="Vyberte stav"
                options={[
                    { label: "Skladem", value: 1 },
                    { label: "Není skladem", value: 0 },
                ]}
                value={filter.skladem || ""}
                onChange={handleChange}
                disableDefault={false}
            />

            <Button icon="plus" onClick={() => navigate('/produkt?action=create')}>
                Vytvořit produkt
            </Button>

        </Container>
        <Container property="max-w-3xl mx-auto px-4">
            {filteredValues.map((entity) => (
                <ProductEntity key={entity.id} entity={entity} handleEdit={handleEdit}/>
            ))}
        </Container>
    </>
    )
}
