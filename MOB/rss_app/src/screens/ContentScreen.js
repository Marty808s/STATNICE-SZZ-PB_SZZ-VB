import React from 'react';
import { View, Text, StyleSheet, FlatList, TouchableOpacity } from 'react-native';
import Colors from '../constants/Colors';
import { getContent, addContent } from '../db/db';
import { useState, useEffect } from 'react';
import { useRoute } from '@react-navigation/native';
import { useNavigation } from '@react-navigation/native';

export default function ContentScreen() {
  const route = useRoute();
  const { id, title, description } = route.params || {};
  const [content, setContent] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigation = useNavigation();

  useEffect(() => {
    if (!id) {
      setError('Chybí ID feedu');
      setLoading(false);
      return;
    }

    (async () => {
      try {
        setLoading(true);
        console.log('Načítám obsah pro feed ID:', id);
        
        // fetch obsahu
        let data = await getContent(id);
        console.log('Existující obsah:', data);
        
        // pokud není obsah, stáhnu nový
        if (!data || data.length === 0) {
          console.log('Stahuji nový obsah...');
          await addContent(id);
          data = await getContent(id);
          console.log('Nově stažený obsah:', data);
        }
        
        setContent(data || []);
        console.log('Finální obsah pro zobrazení:', data);
      } catch (e) {
        console.log('Chyba při načítání obsahu:', e);
        setError('Chyba při načítání obsahu');
      } finally {
        setLoading(false);
      }
    })();
  }, [id]);

  // zobraz detail
  const onPress = (item) => {
    console.log("Pressed", item.title);
    navigation.navigate('ContentDetail', {
      id: item.id,
      title: item.title,
      description: item.description,
      link: item.link,
      published: item.published,
    });
  }

  //entita obsahu
  const renderContent = ({ item }) => (
    <TouchableOpacity style={styles.feedItem} onPress={() => onPress(item)}>
      <Text style={styles.feedTitle}>{item.title}</Text>
      <Text style={styles.feedDescription}>{item.description}</Text>
    </TouchableOpacity>
  );

  if (loading) {
    return (
      <View style={styles.container}>
        <Text style={styles.title}>Načítám...</Text>
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.container}>
        <Text style={styles.title}>Chyba</Text>
        <Text style={styles.errorText}>{error}</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text style={styles.title}>RSS Obsah: {title}</Text>
      {title && (
        <View style={styles.feedInfo}>
          <Text style={styles.feedTitle}>{title}</Text>
          {description && <Text style={styles.feedDescription}>{description}</Text>}
        </View>
      )}
      {content.length === 0 ? (
        <Text style={styles.noContent}>Žádný obsah k zobrazení</Text>
      ) : (
        <FlatList
          data={content}
          renderItem={renderContent}
          keyExtractor={item => String(item.id)}
          style={styles.list}
        />
      )}
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
  feedInfo: {
    backgroundColor: Colors.background,
    padding: 15,
    marginBottom: 10,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: Colors.border,
  },
  errorText: {
    color: Colors.error,
    textAlign: 'center',
    marginTop: 10,
  },
  noContent: {
    textAlign: 'center',
    color: Colors.textSecondary,
    marginTop: 20,
  },
});
