library(readr)
library(dplyr)
library(tidyr)
library(ggplot2)
StudentsPerformance_1_ <- read_csv("C:/Users/Marty/Downloads/StudentsPerformance (1).csv")
View(StudentsPerformance_1_)


data <- StudentsPerformance_1_

data <- data %>% mutate(avg.score = (data$`math score` + data$`reading score` + data$`writing score`)/3)

filtered <- data %>% filter(grepl('high school', data$`parental level of education`))
filtered

data2 <- data %>% group_by(`gender`, `race/ethnicity`) %>% summarise(grouped_avg = mean(avg.score))

wide <- data2 %>% pivot_wider(names_from = gender, values_from=grouped_avg)
wide

#vizualizace
ggplot(data2, aes(x = `race/ethnicity`, y = grouped_avg, fill=`race/ethnicity`)) +
  geom_boxplot() +
  labs(x = "Race/Ethnicity", y = "Average score", title = "Rozdělení avgscore podle race/ethnicity") +
  theme_minimal()
