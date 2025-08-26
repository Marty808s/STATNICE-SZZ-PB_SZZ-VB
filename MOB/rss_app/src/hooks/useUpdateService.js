import { useState, useEffect, useCallback } from 'react';
import updateService from '../services/UpdateService';
import { getSetting } from '../db/db';

export const useUpdateService = () => {
    const [isRunning, setIsRunning] = useState(false);
    const [lastUpdate, setLastUpdate] = useState(null);
    const [updateInterval, setUpdateInterval] = useState(60);
    const [autoUpdateEnabled, setAutoUpdateEnabled] = useState(true);
    const [isLoading, setIsLoading] = useState(false);

    // Načtení nastavení při inicializaci
    useEffect(() => {
        loadSettings();
    }, []);

    // Načtení nastavení z databáze
    const loadSettings = async () => {
        try {
            const interval = await getSetting('update_interval_minutes', '60');
            const enabled = await getSetting('auto_update_enabled', 'true');
            
            setUpdateInterval(parseInt(interval));
            setAutoUpdateEnabled(enabled === 'true');
            
            // Kontrola stavu služby
            const status = updateService.getStatus();
            setIsRunning(status.isRunning);
            setLastUpdate(status.lastUpdate);
        } catch (error) {
            console.error('Chyba při načítání nastavení:', error);
        }
    };

    // Spuštění automatických aktualizací
    const startAutoUpdate = useCallback(async () => {
        try {
            setIsLoading(true);
            await updateService.startAutoUpdate();
            const status = updateService.getStatus();
            setIsRunning(status.isRunning);
            setLastUpdate(status.lastUpdate);
        } catch (error) {
            console.error('Chyba při spuštění automatických aktualizací:', error);
        } finally {
            setIsLoading(false);
        }
    }, []);

    // Zastavení automatických aktualizací
    const stopAutoUpdate = useCallback(() => {
        updateService.stopAutoUpdate();
        setIsRunning(false);
    }, []);

    // Manuální aktualizace
    const manualUpdate = useCallback(async () => {
        try {
            setIsLoading(true);
            await updateService.manualUpdate();
            const status = updateService.getStatus();
            setLastUpdate(status.lastUpdate);
        } catch (error) {
            console.error('Chyba při manuální aktualizaci:', error);
        } finally {
            setIsLoading(false);
        }
    }, []);

    // Změna intervalu aktualizací
    const changeUpdateInterval = useCallback(async (minutes) => {
        try {
            await updateService.setUpdateInterval(minutes);
            setUpdateInterval(minutes);
        } catch (error) {
            console.error('Chyba při změně intervalu:', error);
        }
    }, []);

    // Zapnutí/vypnutí automatických aktualizací
    const toggleAutoUpdate = useCallback(async (enabled) => {
        try {
            await updateService.setAutoUpdateEnabled(enabled);
            setAutoUpdateEnabled(enabled);
            
            if (enabled) {
                await startAutoUpdate();
            } else {
                stopAutoUpdate();
            }
        } catch (error) {
            console.error('Chyba při změně automatických aktualizací:', error);
        }
    }, [startAutoUpdate, stopAutoUpdate]);

    // Aktualizace stavu při změnách
    useEffect(() => {
        const interval = setInterval(() => {
            const status = updateService.getStatus();
            setIsRunning(status.isRunning);
            setLastUpdate(status.lastUpdate);
        }, 1000);

        return () => clearInterval(interval);
    }, []);

    return {
        // Stav
        isRunning,
        lastUpdate,
        updateInterval,
        autoUpdateEnabled,
        isLoading,
        
        // Akce
        startAutoUpdate,
        stopAutoUpdate,
        manualUpdate,
        changeUpdateInterval,
        toggleAutoUpdate,
        
        // Utility
        loadSettings
    };
};
