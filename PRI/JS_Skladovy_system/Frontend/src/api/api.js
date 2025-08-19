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

    return {
        getUsers,
        getProdukty
    };
};