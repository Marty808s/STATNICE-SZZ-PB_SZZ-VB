import React from 'react';
import { View, Text, StyleSheet, FlatList } from 'react-native';
import Colors from '../constants/Colors';

const mockFeeds = [
  { id: '1', title: 'Novinky.cz', description: 'Nejnovější zprávy z ČR' },
  { id: '2', title: 'iDNES.cz', description: 'Zprávy, sport, kultura' },
  { id: '3', title: 'ČT24', description: 'Zpravodajství České televize' },
];

export default function FeedScreen() {
  const renderFeed = ({ item }) => (
    <View style={styles.feedItem}>
      <Text style={styles.feedTitle}>{item.title}</Text>
      <Text style={styles.feedDescription}>{item.description}</Text>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.title}>RSS Feedy</Text>
      <FlatList
        data={mockFeeds}
        renderItem={renderFeed}
        keyExtractor={item => item.id}
        style={styles.list}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.background,
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: Colors.text,
    marginBottom: 20,
    textAlign: 'center',
  },
  list: {
    flex: 1,
  },
  feedItem: {
    backgroundColor: Colors.background,
    padding: 15,
    marginBottom: 10,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: Colors.border,
  },
  feedTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: Colors.text,
    marginBottom: 5,
  },
  feedDescription: {
    fontSize: 14,
    color: Colors.textSecondary,
  },
});
