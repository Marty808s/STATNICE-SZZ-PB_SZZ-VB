import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { StatusBar } from 'expo-status-bar';
import TabNavigator from './src/navigation/TabNavigator';
import { fetchRSSChannel } from './src/api/FetchRSS';
import { useEffect } from 'react';
import { createTables } from './src/db/db';

export default function App() {
  // testovací funkce - zatím init
  useEffect(() => {
    const fetchTest = async() => {
      const res = await fetchRSSChannel("https://servis.idnes.cz/rss.aspx?c=prahah")
      const db = await createTables();
      console.log("DB: ", db);
    }
    fetchTest();
  }, []);

  return (
    <NavigationContainer>
      <TabNavigator />
      <StatusBar style="light" />
    </NavigationContainer>
  );
}
