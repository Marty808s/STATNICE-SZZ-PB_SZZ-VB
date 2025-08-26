import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import FeedScreen from '../screens/FeedScreen';
import ContentScreen from '../screens/ContentScreen';
import ContentDetailScreen from '../screens/ContentDetailScreen';
import Colors from '../constants/Colors';

const Stack = createStackNavigator();

export default function FeedStackNavigator() {
  return (
    <Stack.Navigator
      screenOptions={{
        headerStyle: {
          backgroundColor: Colors.primary,
        },
        headerTintColor: '#FFFFFF',
        headerTitleStyle: {
          fontWeight: 'bold',
        },
      }}
    >
      <Stack.Screen 
        name="FeedList" 
        component={FeedScreen}
        options={{ title: 'RSS Feedy' }}
      />
        <Stack.Screen 
            name="ContentScreen" 
            component={ContentScreen}
            options={{ title: 'Obsah' }}
        />
        <Stack.Screen 
            name="ContentDetail" 
            component={ContentDetailScreen}
            options={{ title: 'Detail obsahu' }}
        />
    </Stack.Navigator>
  );
}