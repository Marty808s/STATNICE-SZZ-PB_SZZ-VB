import React from 'react';
import { View, Text, StyleSheet, FlatList, TouchableOpacity } from 'react-native';
import Colors from '../constants/Colors';
import { getFeeds, addContent } from '../db/db';
import { useState, useEffect } from 'react';


export default function ContentScreen() {
  const [content, setContent] = useState([]);

  useEffect(() => {
    (async () => {
      try {
        const data = await getFeeds();
        setFeeds(data);
        console.log(data);
      } catch (e) {
        console.log('Chyba při načítání feedů', e);
      }
    })();
  }, []);

  // zobraz detail
  const onPress = (item) => {
    console.log("Pressed", item.title);
    item.id && addContent(item.id);
    //tady bude link na content feed
  }
  //entita feedu
  const renderContent = ({ item }) => (
    <TouchableOpacity style={styles.feedItem} onPress={() => onPress(item)}>
      <Text style={styles.feedTitle}>{item.title}</Text>
      <Text style={styles.feedDescription}>{item.description}</Text>
    </TouchableOpacity>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.title}>RSS Feedy</Text>
      <FlatList
        data={feeds}
        renderItem={renderContent}
        keyExtractor={item => String(item.id)}
        style={styles.list}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.background,
    padding: 5,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: Colors.text,
    marginVertical: 20,
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
