import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Ionicons } from '@expo/vector-icons';
import Colors from '../constants/Colors';
import HomeScreen from '../screens/HomeScreen';
import FeedStackNavigator from './FeedStackNavigator';
import { UpdateProvider } from '../services/UpdateService';

const Tab = createBottomTabNavigator();

export default function TabNavigator() {
  return (
    <UpdateProvider>
      <Tab.Navigator
        // společně screenOptions pro stránky
        screenOptions={({ route }) => ({
          //barva dolní navigace - pozadí
          tabBarStyle: {
            paddingHorizontal: "2px",
            paddingTop: 10,
            backgroundColor: Colors.tabBar, // Bílá
          },

          tabBarIcon: ({ focused, color, size }) => {
            let iconName;

            if (route.name === 'Domů') {
              iconName = focused ? 'home' : 'home-outline';
            } else if (route.name === 'Feedy') {
              iconName = focused ? 'newspaper' : 'newspaper-outline';
            } else if (route.name === 'Nastavení') {
              iconName = focused ? 'settings' : 'settings-outline';
            }

            return <Ionicons name={iconName} size={size} color={color} />;
          },
          tabBarActiveTintColor: Colors.primary,
          tabBarInactiveTintColor: Colors.tabBarInactive,
          // Nastavení header baru nahoře
          headerStyle: {
            backgroundColor: Colors.primary, // Barva pozadí header baru
          },
          headerTintColor: '#FFFFFF',   // Barva textu (název stránky a tlačítka)
          headerTitleStyle: {
            fontWeight: 'bold',         // Tučný text názvu
          },
        })}
      >
        <Tab.Screen name="Domů" component={HomeScreen} />
        <Tab.Screen name="Feedy" component={FeedStackNavigator} options={{ headerShown: false }} />
      </Tab.Navigator>
    </UpdateProvider>
  );
}
