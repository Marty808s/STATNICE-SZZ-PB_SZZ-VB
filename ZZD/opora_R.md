# Opora pro práci s R

## 1. Základní datové typy a struktury

``` r
# Číselné, logické, textové a faktorové hodnoty
x_num <- 3.14
x_log <- TRUE
x_chr <- "text"
x_fac <- factor(c("A", "B", "A"))

# Vektor, matice, data.frame, seznam
vec <- c(1,2,3)
mat <- matrix(1:9, nrow=3)
df  <- data.frame(jmeno=c("A","B"), vek=c(20,25))
lst <- list(vec, mat, df)
```

## 2. Operace a podmínky

``` r
# Logické a matematické operace
a <- 5; b <- 2
a + b; a > b; a == b

# Podmínka
if (a > b) {
  print("a je větší než b")
} else {
  print("a není větší")
}

# Smyčka
for (i in 1:3) {
  print(i^2)
}
```

## 3. Funkce

``` r
# Vlastní funkce
muj_soucet <- function(x, y) {
  return(x + y)
}
muj_soucet(10, 5)
```

## 4. Práce s datovými soubory

``` r
library(readr)
library(readxl)

# Načítání
data_csv <- read_csv("soubor.csv")
data_xls <- read_excel("soubor.xlsx")

# Ukládání
write_csv(data_csv, "vystup.csv")
```

## 5. Manipulace s daty (dplyr)

``` r
library(dplyr)

# Výběr, filtrování, řazení
data %>%
  select(jmeno, vek) %>%
  filter(vek > 18) %>%
  arrange(desc(vek))

# Nový sloupec a seskupení
data %>%
  mutate(prumer = (math + reading + writing)/3) %>%
  group_by(gender, race) %>%
  summarise(avg = mean(prumer, na.rm=TRUE))
```

## 6. Reshape (tidyr)

``` r
library(tidyr)

# Pivot do širokého formátu
data_wide <- data_long %>%
  pivot_wider(names_from = gender, values_from = avg)
```

## 7. Vizualizace (ggplot2)

``` r
library(ggplot2)

# Histogram
ggplot(data, aes(x = avg)) + geom_histogram(bins=20)

# Boxplot
ggplot(data, aes(x = race, y = avg)) + geom_boxplot()
```

## 8. Reporty

-   **R Markdown (Rmd/qmd):**
    -   umožňuje kombinovat text, kód a výstupy,
    -   výsledkem je HTML nebo PDF.
-   Struktura:
    -   **YAML hlavička** (název, autor, formát výstupu),

    -   **textové části** s vysvětlením,

    -   **chunky kódu**:

            ```{r}
            summary(data)

        \`\`\`

------------------------------------------------------------------------

📌 Doporučené zdroje (viz příloha):\
- [R snadno a rychle -- část
1](https://oeconomica.vse.cz/wp-content/uploads/Danko_Safr_R-snadno-a-rychle_1.pdf)\
- [R snadno a rychle -- část
2](https://oeconomica.vse.cz/wp-content/uploads/Safr_Danko_R-snadno-a-rychle_2.pdf)\
- [Cheat sheets
(DataCamp)](https://images.datacamp.com/image/upload/v1697642178/Marketing/Blog/R_Cheat_Sheet_PNG_1.pdf)
