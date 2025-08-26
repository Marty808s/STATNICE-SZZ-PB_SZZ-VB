import React from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, Linking } from 'react-native';
import Colors from '../constants/Colors';
import { useRoute } from '@react-navigation/native';

export default function ContentDetailScreen() {
  const route = useRoute();
  const { id, title, description, link, published, comments } = route.params || {};

  // otevření odkazu v prohlížeči
  const openLink = async () => {
    if (link) {
      try {
        await Linking.openURL(link);
      } catch (error) {
        console.log('Chyba při otevírání odkazu:', error);
      }
    }
  };

  // formátování data
  const formatDate = (dateString) => {
    if (!dateString) return 'Datum není k dispozici';
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('cs-CZ', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      return dateString;
    }
  };

  if (!id) {
    return (
      <View style={styles.container}>
        <Text style={styles.errorText}>Chybí ID článku</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>

      {/* hlavní obsah */}
      <View style={styles.content}>
        <Text style={styles.title}>{title || 'Bez názvu'}</Text>
        
        {/* metadata */}
        <View style={styles.metadata}>
          {published && (
            <View style={styles.metaItem}>
              <Text style={styles.metaLabel}>Publikováno:</Text>
              <Text style={styles.metaValue}>{formatDate(published)}</Text>
            </View>
          )}
          
          {link && (
            <TouchableOpacity style={styles.linkButton} onPress={openLink}>
              <Text style={styles.linkText}>Otevřít v prohlížeči</Text>
            </TouchableOpacity>
          )}
        </View>

        {/* popis článku */}
        {description && (
          <View style={styles.descriptionContainer}>
            <Text style={styles.descriptionLabel}>Popis:</Text>
            <Text style={styles.description}>{description}</Text>
          </View>
        )}

        {/* komentáře */}
        {comments && (
          <View style={styles.commentsContainer}>
            <Text style={styles.commentsLabel}>Komentáře:</Text>
            <Text style={styles.comments}>{comments}</Text>
          </View>
        )}

        {/* id - debug info */}
        <View style={styles.debugInfo}>
          <Text style={styles.debugText}>ID článku: {id}</Text>
        </View>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.background,
  },
  header: {
    padding: 15,
    borderBottomWidth: 1,
    borderBottomColor: Colors.border,
    backgroundColor: Colors.primary,
  },
  backButton: {
    paddingVertical: 8,
    paddingHorizontal: 12,
  },
  backButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  content: {
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: Colors.text,
    marginBottom: 20,
    lineHeight: 32,
  },
  metadata: {
    marginBottom: 25,
    padding: 15,
    backgroundColor: '#F8F9FA',
    borderRadius: 8,
    borderWidth: 1,
    borderColor: Colors.border,
  },
  metaItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 10,
  },
  metaLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: Colors.textSecondary,
  },
  metaValue: {
    fontSize: 14,
    color: Colors.text,
  },
  linkButton: {
    backgroundColor: Colors.primary,
    paddingVertical: 12,
    paddingHorizontal: 20,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 10,
  },
  linkText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  descriptionContainer: {
    marginBottom: 25,
  },
  descriptionLabel: {
    fontSize: 16,
    fontWeight: '600',
    color: Colors.text,
    marginBottom: 10,
  },
  description: {
    fontSize: 16,
    color: Colors.text,
    lineHeight: 24,
    textAlign: 'justify',
  },
  commentsContainer: {
    marginBottom: 25,
    padding: 15,
    backgroundColor: '#F8F9FA',
    borderRadius: 8,
    borderWidth: 1,
    borderColor: Colors.border,
  },
  commentsLabel: {
    fontSize: 16,
    fontWeight: '600',
    color: Colors.text,
    marginBottom: 10,
  },
  comments: {
    fontSize: 14,
    color: Colors.textSecondary,
    lineHeight: 20,
  },
  debugInfo: {
    marginTop: 30,
    padding: 10,
    backgroundColor: '#F0F0F0',
    borderRadius: 4,
  },
  debugText: {
    fontSize: 12,
    color: Colors.textSecondary,
    textAlign: 'center',
  },
});
