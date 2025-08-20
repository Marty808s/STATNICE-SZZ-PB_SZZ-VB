import React from 'react'

export const useAPI = () => {

    const getUsers = async () => {
        try {
            const res = await fetch('http://localhost:8081/api/users');
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání nabídek:", error);
            throw error;
        }
    };

    const getProdukty = async () => {
        try {
            const res = await fetch('http://localhost:8081/api/produkty');
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání nabídek:", error);
            throw error;
        }
    };

    const getObjednavky = async () => {
        try {
            const res = await fetch('http://localhost:8081/api/objednavky');
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání nabídek:", error);
            throw error;
        }
    };

    const getObjednavkaDetail = async (id) => {
        try {
            const res = await fetch(`http://localhost:8081/api/objednavky/${id}`);
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání detailu objednávky:", error);
            throw error;
        }
    };

    const getZakaznici = async () => {
        try {
            const res = await fetch('http://localhost:8081/api/zakaznici');
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání zákazníků:", error);
            throw error;
        }
    };

    const createObjednavka = async (payload) => {
        try {
            const res = await fetch('http://localhost:8081/api/objednavky', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při vytváření objednávky:", error);
            throw error;
        }
    };

    const addProduktyDoObjednavky = async (orderId, items) => {
        try {
            const res = await fetch(`http://localhost:8081/api/objednavky/${orderId}/produkty`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ items })
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při přidávání produktů do objednávky:", error);
            throw error;
        }
    };

    const updateObjednavka = async (orderId, payload) => {
        try {
            const res = await fetch(`http://localhost:8081/api/objednavky/${orderId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při úpravě objednávky:", error);
            throw error;
        }
    };

    const updateProduktyObjednavky = async (orderId, items) => {
        try {
            const res = await fetch(`http://localhost:8081/api/objednavky/${orderId}/produkty`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ items })
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při úpravě produktů objednávky:", error);
            throw error;
        }
    };

    const getProdukt = async (id) => {
        try {
            const res = await fetch(`http://localhost:8081/api/produkty/${id}`);
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání produktu:", error);
            throw error;
        }
    };

    const createProdukt = async (payload) => {
        try {
            const res = await fetch('http://localhost:8081/api/produkty', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při vytváření produktu:", error);
            throw error;
        }
    };

    const updateProdukt = async (id, payload) => {
        try {
            const res = await fetch(`http://localhost:8081/api/produkty/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!res.ok) throw new Error(`HTTP ${res.status}`)
            return await res.json();
        } catch (error) {
            console.error("Chyba při úpravě produktu:", error);
            throw error;
        }
    };


    return {
        getUsers,
        getProdukty,
        getObjednavky,
        getObjednavkaDetail,
        getZakaznici,
        createObjednavka,
        addProduktyDoObjednavky,
        updateObjednavka,
        updateProduktyObjednavky,
        getProdukt,
        createProdukt,
        updateProdukt
    };
};