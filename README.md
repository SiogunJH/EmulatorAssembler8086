# 8086 Simulator
#### Project by Hojda via CSharp

# Dostępne Instrukcje

### AAA
Składnia:
> AAA

Instrukcja AAA służy do korekcji wyniku dodawania dwóch liczb w *rozpakowanym kodzie BCD* zapisanym w *operandzie domyślnym*. 

Operandem domyślnym jest rejestr **AX**

### AAD
Składnia:
> AAD

### AAM
Składnia:
> AAM

### AAS
Składnia:
> AAS

Instrukcja AAA służy do korekcji wyniku odejmowania dwóch liczb w *rozpakowanym kodzie BCD* zapisanym w *operandzie domyślnym*.

Operandem domyślnym jest rejestr **AX**

### ADC
Składnia:
> ADC operand1, operand2

### ADD
Składnia:
> ADD operand1, operand2

Instrukcja ADD służy do dodania wartości *operanda drugiego* do wartości *operanda pierwszego* i zapisania wyniku na adresie *operanda pierwszego*

### CBW
Składnia:
> CBW

### CWD
Składnia:
> CWD

### DAA
Składnia:
> DAA

### DAS
Składnia:
> DAS

### DEC
Składnia:
> DEC operand1

Instrukcja INC służy do zmniejszenia wartości *operanda pierwszego* o *1* i zapisania wyniku na adresie *operanda pierwszego*

### DIV
Składnia:
> DIV operand1

Instrukcja DIV służy do dzielenia *operanda domyślnego* przez wartość *operanda pierwszego*. Jesli podany operand jest 8-bitowy, operand domyślny przyjmuje wartość rejestru *AX*, a wyniki zapisywane są w rejestrach AH (modulo) oraz AL (iloraz). Jesli podany operand jest 16-bitowy, operand domyślny przyjmuje 32-bitową wartość wynikająca z zestawienia rejestró *DX* i *AX*, a wyniki zapisywane są w rejestrach DX (modulo) oraz AX (iloraz).

### IDIV
Składnia:
> IDIV operand1

Instrukcja IDIV jest wariantem funkcji DIV - działa względem niej identycznie, z tą różnicą, że poprawnie interpretuje liczby ujemne.

### IMUL
Składnia:
> IMUL operand1

Instrukcja IMUL jest wariantem funkcji MUL - działa względem niej identycznie, z tą różnicą, że poprawnie interpretuje liczby ujemne.

### INC
Składnia:
> INC operand1

Instrukcja INC służy do zwiększenia wartości *operanda pierwszego* o *1* i zapisania wyniku na adresie *operanda pierwszego*

### LAHF
Składnia:
> LAHF

Instrukcja LAHF służy do przypisania rejestrowi **AH** wartości, utworzonej na podstawie rejestru flag. Poszczególne flagi oznaczają wartości pojedynczych bitów, w następującym porządku:  *[SF], [ZF], [0], [AF], [0], [PF], [1], [CF]*. Przykład: Jeśli flaga AF i SF ma wartość 1, a pozostałe flagi wartość 0, wynikiem instrukcji LAHF będzie przypisanie do rejestru AH wartości *1001 0010*, czyli *92* (w formacie szesnastkowym).

### LEA
Składnia:
> LEA operand1, operand2

Instrukcja LEA służy do przeniesienia wartości *operanda drugiego* (źródła) do adresu *operanda drugiego* (celu). W przeciwieństwie do MOV, instrukcja ta ma ograniczenia co do typu operandów. Jako operand pierwszy, musi zostać podany **wskaźnik**, a jako operand drugi, musi zostać podany **rejestr z końcówką X**, **wskaźnik** lub **pamięć**. Dodatkowo, jeśli operandem drugim jest **pamięć**, nie jest kopiowana wartość przechowywana pod danym adresem, tylko **adres pamięci**.

### MOV
Składnia:
> MOV operand1, operand2

Instrukcja MOV służy do przeniesienia wartości *operanda drugiego* (źródła) do adresu *operanda pierwszego* (celu)

### MUL
Składnia:
> MUL operand1

Instrukcja MUL służy do mnożenia *operanda pierwszego* i *domyślnego operanda*. W zależności od tego, czy operacja przeprowadzana jest na liczbach *8-bitowych* czy *16-bitowych*, wynik zapisywany jest do *AX* lub *AX i DX*.

Operandem domyślnym jest rejestr **AL**, dla operacji 8-bitowych lub rejestr **AX** dla operacji 16-bitowych

### NEG
Składnia:
> NEG operand1

### POP
Składnia:
> POP operand1

Instrukcja POP pobiera wartość ze stosu (na adresie *[SP]*) i umieszcza ją na adresie *operanda pierwszego*, po czym zmienia wartość *[SP]* na *[SP]+2*. Operand pierwszy może byc wyłącznie typu *rejestr z końcówką X*, *rejestr segmentu*, *rejestr wskaźnika*, *pamięć*.

### POPF
Składnia:
> POPF

Instrukcja POPF działa dokładnie tak jak instrukcja SAHF, jednak zamiast pobierać wartość utworzoną na podstawie flag z rejestru *AH*, pobiera ją ze stosu

### PUSH
Składnia:
> PUSH operand1

Instrukcja PUSH pobiera wartość *operanda pierwszego* i umieszcza ją na adresie *[SP]-2*, po czym zmienia wartość *[SP]* na *[SP]-2*. Operand pierwszy może byc wyłącznie typu *rejestr z końcówką X*, *rejestr segmentu*, *rejestr wskaźnika*, *pamięć*.

### PUSHF
Składnia:
> PUSHF

Instrukcja PUSHF działa dokładnie tak jak instrukcja LAHF, jednak zamiast umieszczać wartość utworzoną na podstawie flag w rejestrze *AH*, umieszcza ją na stosie

### SAHF
> SAHF

Instrukcja SAHF służy do przeniesienia binarnej wartości rejestru **AH** do rejestru flag w następującym porządku bitów: *[SF], [ZF], [-], [AF], [-], [PF], [-], [CF]*. Przykład: Jeśli wartość rejestru **AH** wynosi *1000 0001*, to flagi *CF* oraz *SF* zostaną ustawione na **1**, a wszystkie pozostałe na **0**.

### SBB
Składnia:
> SBB operand1, operand2

### SUB
Składnia:
> SUB operand1, operand2

Instrukcja SUB służy do odjęcia wartości *operanda drugiego* od wartości *operanda pierwszego* i zapisania wyniku na adresie *operanda pierwszego*

### XCHG
Składnie:
> XCHG operand1, operand2

Instrukcja XCHG służy do *wymiany wartości* między operandami. Operand pierwszy przyjmuje wartość drugiego, a operand drugi przyjmuje wartość pierwszego

# Dostępna Pamięć
## Rejestr Podstawowy
Rejestr składa się z czterech **16-bitowych komórek**, oznaczonych literami **A**, **B**, **C** i **D** z końcówką **X** (np. CX).
Dodatkowo, każda komórka rejestru dzieli się na dwie 8-bitowe komórki, z końcówkami kolejno **H** oraz **L** (np. BH i BL).
Można więc edytować dowolny rejestr 16-bitowy wpisując dwie liczby 8-bitowe.
Przykładowo, przypisując dla **AH** wartość *FA*, a dla **AL** wartość *12*, to wartość **AX** będzie wynosić *FA12*.

<table>
    <thead>
        <tr>
            <th colspan=2>AX</th>
            <th colspan=2>BX</th>
            <th colspan=2>CX</th>
            <th colspan=2>DX</th>
        </tr>
        <tr>
            <th>AH</th>
            <th>AL</th>
            <th>BH</th>
            <th>BL</th>
            <th>CH</th>
            <th>CL</th>
            <th>DH</th>
            <th>DL</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
            <td>8-bit</td>
        </tr>
    </tbody>
</table>

## Rejestr Segmentów
Rejestr segmentów składa się z trzech **16-bitowych komórek**, oznaczonych **SS**, **DS** i **ES**.

|SS|DS|ES|
|--|--|--|
|16-bit|16-bit|16-bit|

**Segment Stosu** (SS, z ang. *Stack Segment*) - ZASTOSOWANIE

**Segment Danych** (DS, z ang. *Data Segment*) - ZASTOSOWANIE

**Segment Dodatkowy** (ES, z ang. *Extra Segment*) - ZASTOSOWANIE

## Rejestr Wskaźników
Rejestr wskaźników składa się z czterech **16-bitowych komórek**, oznaczonych **SP**, **BP**, **SI** i **DI**.

|SP|BP|SI|DI|
|--|--|--|--|
|16-bit|16-bit|16-bit|16-bit|

**Wskaźnik Stosu** (SP, z ang. *Stack Pointer*) - ZASTOSOWANIE

**Wskaźnik Bazowy** (BP, z ang. *Base Pointer*) - ZASTOSOWANIE

**Indeks Źródła** (SI, z ang. *Source Index*) - ZASTOSOWANIE

**Indeks Celu** (DI, z ang. *Destination Index*) - ZASTOSOWANIE


## Flagi
Rejestr flag składa się z dziewięciu **1-bitowych komórek**, oznaczonych **OF**, **DF**, **IF**, **TF**, **SF**, **ZF**, **AF**, **PF** i **CF**.

|OF|DF|IF|TF|SF|ZF|AF|PF|CF|
|--|--|--|--|--|--|--|--|--|
|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|

**Flaga Przelania** (OF, z ang. *Overflow Flag*) - ZASTOSOWANIE

**Flaga Kierunkowa** (DF, z ang. *Directional Flag*) - ZASTOSOWANIE

**Flaga Przerwania** (IF, z ang. *Interruption Flag*) - ZASTOSOWANIE

**Flaga Pułapki** (TF, z ang. *Trap Flag*) - ZASTOSOWANIE

**Flaga Znaku** (SF, z ang. *Sign Flag*) - odpowiada za określenie, czy wartość jest dodatnia czy ujemna. Przyjmuje wartość **1** dla liczb ujemnych i wartość **0** dla dodatnich. Rozróżnianie znaku liczby opiera się na **wartości najwyższego bitu liczby**. Jeśli owy bit jest równy 1, liczba uznawana jest za ujemną - prowadzi to do sytuacji, gdzie dodając dwie liczby dodatnie, np. *70h (**0**111 0000)* i *60h (**0**110 0000)*, możemy uzyskać liczbę ujemną *D0h (**1**101 0000)*.

**Flaga Zera** (ZF, z ang. *Zero Flag*) - ZASTOSOWANIE

**Flaga Dopasowania** (AF, z ang. *Auxiliary Carry Flag*) - przyjmuje wartość **1**, jeśli w wyniku operacji arytmetycznej lub logicznej, działania przeprowadzone na **niższych półbajtach** nie mieszczą się w swoim obrębie i wymagają pobrania lub dodania jakiejś wartości do **wyższych półbajtów**.
Przykład: DODAĆ PRZYKŁAD

**Flaga Parzystości** (PF, z ang. *Parity Flag*) - wykorzystywana przy wykonywaniu obliczeń arytmetycznych. Jeśli ilość jedynek w danej liczbie binarnej jest parzysta, flaga przyjmuje wartość **1** - w przeciwnym wypadku przypisywana jest wartość **0**

**Flaga Przeniesienia** (CF, z ang. *Carry Flag*) - ZASTOSOWANIE
<!-- przyjmuje wartość **1** jeśli w wyniku wykonania instrukcji, jej wynik nie będzie mieścił się w komórce zapisu (*0-255* dla komórek 8-biotwych i *0-65535* dla komórek 16-bitowych). W przeciwnym wypadku flaga przyjmie wartość **0**. Przykład: W wyniku odejmowania komórek *AH* (o wartości *64*) oraz *AL* (o wartości *128*), do komórki AH ma zostać zapisany wynik *-64*. Wynik ten nie mieści się w obrębie *0-255*, więc musi zostać odpowiednio dostosowany - dodawana jest do niego wartość *256*, a flaga *CF* ustawiana jest na *1*, więc końcowa wartość AH wynosi *192*
 -->

# Struktura Plików
#### Base.cs
Rrozpoczyna pracę programu.
Odpowiada za pracę konsoli i jej zapętlanie.

#### Command.cs
Odpowiada za rozpoznanie polecenia i wysłanie informacji zwrotnej.

#### Algorithms.cs
Odpowiada za wykonywanie złożonych instrukcji.

#### Storage.cs
Odpowiada za utworzenie struktur i zmiennych przechowujących dane, które mają być dostępne z dowolnego miejsca w kodzie.

#### Tools.cs
Posiada przydatne funkcje.