import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import Colors from '../constants/Colors';
import { resetDB, addContent } from '../db/db';
import { useEffect } from 'react';

export default function HomeScreen() {
  useEffect(() => {
    (async () => {
      console.log("Adding content");
      await addContent(1);
    })();
  }, []);

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Domů</Text>
      <Text style={styles.subtitle}>Vítejte v RSS aplikaci</Text>
      <TouchableOpacity style={styles.button} onPress={() => resetDB()}>
        <Text style={styles.buttonText}>RESETOVAT DB</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: Colors.text,
    marginBottom: 10,
  },
  subtitle: {
    fontSize: 16,
    color: Colors.textSecondary,
  },
  button: {
    backgroundColor: Colors.primary,
    padding: 15,
    borderRadius: 8,
    marginTop: 20,
    alignItems: 'center',
  },
  buttonText: {
    color: Colors.background,
    fontSize: 16,
    fontWeight: '600',
  },
});
