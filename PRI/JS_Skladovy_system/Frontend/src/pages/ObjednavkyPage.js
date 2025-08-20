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
import ObjednavkaEntity from "@components/Entity/ObjednavkaEntity";

export default function ObjednavkyPage() {
    const [data, setData] = useState([]);
    const [filter, setFilter] = useState({ nazev: "", stav: "" }); 
    const { getObjednavky } = useAPI();
    const [searchParams, setSearchParams] = useSearchParams();
    const navigate = useNavigate();

    const handleChange = (patch) => {
        setFilter((prev) => ({ ...prev, ...patch }))
    }

    const initFetch = async() => {
        const res = await getObjednavky();
        setData(res);
    }

    //filtrované hodnoty 
    const normalizedFilter = (filter.nazev || "").toLowerCase()
    const normalizedState = (filter.stav || "").toLowerCase()
    const filteredValues = Array.isArray(data)
        ? data.filter((entity) => {
            // Filtrování podle ID, jména nebo příjmení
            const searchMatch = !normalizedFilter || 
                entity.id?.toString().includes(normalizedFilter) ||
                (entity.name || "").toLowerCase().includes(normalizedFilter) ||
                (entity.surname || "").toLowerCase().includes(normalizedFilter)
            
            // Filtrování podle stavu
            const stateMatch = !normalizedState || 
                (entity.state || "").toLowerCase().includes(normalizedState)
            
            // Entity musí odpovídat VŠEM aktivním filtrům (AND logika)
            return searchMatch && stateMatch
        })
        : []

    useEffect(() => {
        initFetch();
    }, [])

    // init načtení parametrů
    useEffect(() => {
        const nazevParam = searchParams.get('nazev') || "";
        const stavParam = searchParams.get('stav') || "";
        if (nazevParam !== "" || stavParam !== "") {
            setFilter((prev) => ({ ...prev, nazev: nazevParam, stav: stavParam }));
        }
    }, [])

    // změna parametrů v url
    useEffect(() => {
        const params = {};
        if (filter.nazev) params.nazev = filter.nazev;
        if (filter.stav !== "" && filter.stav !== null && filter.stav !== undefined) {
            params.stav = String(filter.stav);
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
        console.log("Kliknutí na entity:", entity.id);
        navigate(`/objednavky/${entity.id}`);
    }

    const handleViewDetail = (e, entity) => {
        e.stopPropagation();
        console.log("Zobrazení detailu entity:", entity.id);
        navigate(`/objednavky/${entity.id}`);
    }

    return (
    <>
        <Nav/>
        <Container property="flex w-full justify-center m-4">
            <Headings>Objednávky</Headings>
        </Container>
        <Container property="w-full flex justify-center m-4 gap-2" >
            <TextField 
                id="nazev" 
                property="w-64" 
                placeholder="Hledejte podle id, jména, příjmení" 
                value={filter.nazev || ""}
                onChange={handleChange}
            />
            <DropDown
                id="stav"
                placeholder="Stav objendávky"
                options={[
                    { label: "Zpracování", value: "Zpracování" },
                    { label: "Export", value: "Export" },
                    { label: "Dokončená", value: "Dokončená" },
                ]}
                value={filter.stav || 0}
                onChange={handleChange}
                disableDefault={false}
            />
        </Container>
        <Container property="max-w-3xl mx-auto px-4">
            {filteredValues.map((entity) => (
                <ObjednavkaEntity 
                    key={entity.id} 
                    entity={entity} 
                    handleEdit={handleEdit}
                    handleViewDetail={handleViewDetail}
                />
            ))}
        </Container>
    </>
    )
}
