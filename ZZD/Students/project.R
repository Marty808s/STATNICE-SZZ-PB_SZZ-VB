library(dplyr)
library(tidyr)
library(stringr)
library(ggplot2)

library(readr)
dataset <- read_csv('../StudentsPerformance.csv') #načtení dat
View(dataset)

# vytvořte nový sloupec avg.score, který bude pro každý záznam tabulky obsahovat
#průměrné skóre dané osoby ze sloupců math.score, reading.score
#a writing.score,
data <- dataset

data <- mutate(data, avgscore = (data$`math score` + data$`reading score` + data$`writing score`)/3)
   

# z tabulky vyfiltrujte pouze záznamy, kde sloupec parental.level.of.education
#obsahuje pojem “high school” kdekoliv v textu,

data %>% filter(grepl('high school', data$`parental level of education`))

# vypočtěte průměrnou hodnotu sloupce avg.score pro každou kombinaci kategorií
#gender a race.ethnicity,

res <- data %>%
  group_by(gender, `race/ethnicity`) %>%
  summarise(mean_avg = mean(avgscore, na.rm = TRUE)) %>%
    arrange(desc(mean_avg))
res

# tabulku upravte do tzv. “širokého formátu” tak, aby kategorie ze sloupce gender
#představovaly vlastní sloupce a vypočtené průměrné hodnoty byly hodnotami v těchto nových
#sloupcích,
wide <- data %>%
  group_by(gender, `race/ethnicity`) %>%
  summarise(mean_avg = mean(avgscore, na.rm = TRUE), .groups = "drop") %>%
  ungroup() %>%
  pivot_wider(
    names_from  = gender,          # sloupce: kategorie pohlaví
    values_from = mean_avg,        # hodnoty: průměr avg.score
    names_prefix = "avg_",         # volitelně: prefix do názvů sloupců
    values_fill = NA_real_         # co když chybí kombinace
  )

wide

# pro každou kategorii ze sloupce race.ethnicity vytvořte boxplot ze sloupce avg.score.

ggplot(data, aes(x = `race/ethnicity`, y = avgscore)) +
  geom_boxplot() +
  labs(x = "Race/Ethnicity", y = "Average score", title = "Rozdělení avgscore podle race/ethnicity") +
  theme_minimal()

