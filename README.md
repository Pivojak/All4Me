# All4Me
Osobní evidence základních potřeb. Jako je správa financí, záznam práce a poznámkový blok v jedné aplikaci.
# Základní popis
Aplikace byla napsáno pro usnadnění správy financí a záznamů práce. Na úvodní obrazovce - jak ukazuje soubor MainWindow.png, je základní    rozdělení aplikace. V této části je možné si vybrat jaký modul uživatel chce aktuálně využívat. Na výběr je Evidence financí, Evidence      práce a poznámkový blok. Po zakliknutí jednoho z obrázku se spustí příslušný modul.
*****************************************************************************************************************************************
# Modul poznámkového bloku
V této části si uživatel může zapsat libovolné poznámky, které potřebuje zaznamenat. Může zde mít až 20 poznámek, rozdělených po 4 na 5 stran. Pro každou ze stran si může nastavit jednu z 5 barev, tak aby měl poznámky rozdělené třeba na osobní, pracovní, rodinné apod. Mezi stranami se poté přepíná pomocí horního panelu, kliknutím na jedno z pěti čísel. Přidáním poznámky se poznámka zařadí za stávající, tedy pokud jsou na první straně 3 poznámky, doplní první stranu a poté začnou poznámky přibývat na další straně apod. Vzhled úvodní obrazovky ukazuje NoteMainWindow.png v tomto adresáři.
*****************************************************************************************************************************************
# Modul financí
Základní evidence příjmů a výdajů pro běžného uživatele. Hlavní obrazovka jak ukazuje FinanceRegisterMainWindow.png obsahuje sloupec transakcí, které jsou rozděleny na dvě strany a mění se pomocí kolečka. Každá transakce zobrazuje částku, datum, název a barevné oddělení pro příjmy (zelená) a výdaje (červené). Editace transakce probíhá pomocí kliknutí na některou transakci levým tlačítkem myši. Je vyvoláno okno, která umožní změnit všechny parametry transakce. Nad tímto seznamem transakcí jsou vidět zůstatky na bankovním účtu a v hotovosti. V levé části je hlavní menu, které začíná tlačítkem PŘEHLED, které uvede modul do základní podoby, tedy tak jak ukazuje obrázek. Následující tlačítko umožňuje přidat transakci. Další tlačítko vybírá transakce podle několika parametrů, v novém okně si uživatel vybere zda chce zobrazit transakce podle měsíce, kategorie, typů zůstatku (bankovní účet, hotovost) a nebo typu transakce (příjmy, výdaje). Může zvolit všechny parametry a aplikace záznamy vyhledá. Také je zde možnost vybrané záznamy vytisknout a to txt souboru. Tisk nabízí dvě varianty, a to tisk ve formě, aby se dali vložit do Wordu nebo do Excelu. Tlačítko STATISTIKA, vyvolá tři tlačítka, kde uživatel zvolí zda chce měsíční přehled, roční a nebo výdaje dle kategorií. První dva zobrazí řádkový výpis požadovaných hodnost, kategorie zobrazí sloupcový graf rozdělený dle kategorií. Tlačítko tisk má stejnou funkci jako tlačítko tisk u výběrů, avšak v tomto případě vytiskne všechny transakce. 
*****************************************************************************************************************************************
# Modul práce
Slouží pro zápis denních záznamů práce a umožňuje práci evidovat na dva denní bloky např. dopolední a odpolední. Součástí je možnost vytvoření projektu na kterém uživatel aktuálně pracuje a evidovat si denní záznamy přímo k tomuto projektu, tak aby v přehledu bylo jasně vidět co který den uživatel dělal a jakého projektu se práce týkala. Hlavní obrazovka jak ukazuje WorkRegisterMainWindow.png je vybavena vykreslovací plochou po pravé straně, kde jsou vidět jednotlivé záznamy. Nad nimi je poté dvojice comboBoxů pro výběr měsíce a roku, pro který se mají záznamy zobrazit. Záznamy je možné rolovat, a to až na 5 stran pomocí kolečka v obou směrech. Každý záznam je rozdělen na dvé částí (zmíněné denní bloky), kde každý blok obsahuje informaci od - do a práci, na které uživatel pracoval. Denní blok poté uzavírá informace o odpracovaných hodinách a název projektu. Blok má poté na své pravé straně tří tlačítka pro ovládání, první přidá jeden blok (dopolední např.), druhé umožňuje správu dne, tedy nastavení projektu, plánovaného počtu hodin a také úpravu obou pracovních bloků. Poslední tlačítko smaže postupně bloky a následně i celý denní záznam. Vlevé části je základní menu, které začíná tlačítkem, jež uživatele přepne na základní zobrazení. Následuje tlačítko pro přidání projektu nebo denního záznamu a tlačítko projektů, kde je následně vyžádáno vybraní projektu, který má uživatel v plánu zobrazit / upravit. Menu uzavírá tlačítko Vyber, které zobrazuje pracovní statistku, tedy buď přehled za rok - zobrazí 12 bloků rozdelených na dvě strany, dle počtu měsíců a nebo měsíční přehled, kde se budou rozbrazeny statistiky za týdny. Přesun mezi stranami je poté možný pomocí rotace kolečka. 
*****************************************************************************************************************************************
