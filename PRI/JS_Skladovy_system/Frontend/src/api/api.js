export const api = () => {
    const getUsers = async () => {
        try {
            const res = await fetch('/api/users');
            if (!res.ok) {
                throw new Error(`HTTP ${res.status}`);
            }
            return await res.json();
        } catch (error) {
            console.error("Chyba při získávání uživatelů:", error);
            throw error;
        }
    };

    return { getUsers };
};