import { XMLParser } from 'fast-xml-parser';

// získání kanálu RSS - a pak převod na JSON
export async function fetchRSSChannel(url) {
    const response = await fetch(url);
    const text = await response.text();
    const parser = new XMLParser();
    const json = parser.parse(text);
    console.log("JSON: ", json.rss.channel);
    return json;
}