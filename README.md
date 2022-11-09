# 8086 Simulator
#### Project by Hojda via CSharp

# Dostępne Instrukcje

### AAA
Składnia:
> AAA

Test

### AAD
Składnia:
> AAD

### AAM
Składnia:
> AAM

### AAS
Składnia:
> AAS

### ADC
Składnia:
> ADC operand1, operand2

### ADD
Składnia:
> ADD operand1, operand2

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

### DIV
Składnia:
> DIV operand1

### INC
Składnia:
> INC operand1

### MOV
Składnia:
> MOV operand1, operand2

### NEG
Składnia:
> NEG operand1

### MUL
Składnia:
> MUL operand1

### SBB
Składnia:
> SBB operand1, operand2

### SUB
Składnia:
> SUB operand1, operand2

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

**Wskaźnik Stosu** (SP, z ang. *Stack Pointer*) - ZASTOSOWANIE<br>
**Wskaźnik Bazowy** (BP, z ang. *Base Pointer*) - ZASTOSOWANIE<br>
**Indeks Źródła** (SI, z ang. *Source Index*) - ZASTOSOWANIE<br>
**Indeks Celu** (DI, z ang. *Destination Index*) - ZASTOSOWANIE<br>

## Flagi
Rejestr flag składa się z dziewięciu **1-bitowych komórek**, oznaczonych **OF**, **DF**, **IF**, **TF**, **SF**, **ZF**, **AF**, **PF** i **CF**.

|OF|DF|IF|TF|SF|ZF|AF|PF|CF|
|--|--|--|--|--|--|--|--|--|
|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|1-bit|

**Flaga Przelania** (OF, z ang. *Overflow Flag*) - ZASTOSOWANIE<br>
**Flaga Kierunkowa** (DF, z ang. *Directional Flag*) - ZASTOSOWANIE<br>
**Flaga Przerwania** (IF, z ang. *Interruption Flag*) - ZASTOSOWANIE<br>
**Flaga Pułapki** (TF, z ang. *Trap Flag*) - ZASTOSOWANIE<br>
**Flaga Znaku** (SF, z ang. *Sign Flag*) - odpowiada za określenie, czy wartość jest dodatnia czy ujemna. Przyjmuje wartość **1** dla liczb ujemnych i wartość **0** dla dodatnich. Rozróżnianie znaku liczby opiera się na **wartości najwyższego bitu liczby**. Jeśli owy bit jest równy 1, liczba uznawana jest za ujemną - prowadzi to do sytuacji, gdzie dodając dwie liczby dodatnie, np. *70h (**0**111 0000)* i *60h (**0**110 0000)*, możemy uzyskać liczbę ujemną *D0h (**1**101 0000)*.<br>
**Flaga Zera** (ZF, z ang. *Zero Flag*) - ZASTOSOWANIE<br>
**Flaga Dopasowania** (AF, z ang. *Auxiliary Carry Flag*) - przyjmuje wartość **1**, jeśli w wyniku operacji arytmetycznej lub logicznej, działania przeprowadzone na **niższych półbajtach** nie mieszczą się w swoim obrębie i wymagają pobrania lub dodania jakiejś wartości do **wyższych półbajtów**.<br>Przykład: DODAĆ PRZYKŁAD<br>
**Flaga Parzystości** (PF, z ang. *Parity Flag*) - wykorzystywana przy wykonywaniu obliczeń arytmetycznych. Jeśli ilość jedynek w danej liczbie binarnej jest parzysta, flaga przyjmuje wartość **1** - w przeciwnym wypadku przypisywana jest wartość **0**<br>
**Flaga Przeniesienia** (CF, z ang. *Carry Flag*) - ZASTOSOWANIE
<!-- przyjmuje wartość **1** jeśli w wyniku wykonania instrukcji, jej wynik nie będzie mieścił się w komórce zapisu (*0-255* dla komórek 8-biotwych i *0-65535* dla komórek 16-bitowych). W przeciwnym wypadku flaga przyjmie wartość **0**. Przykład: W wyniku odejmowania komórek *AH* (o wartości *64*) oraz *AL* (o wartości *128*), do komórki AH ma zostać zapisany wynik *-64*. Wynik ten nie mieści się w obrębie *0-255*, więc musi zostać odpowiednio dostosowany - dodawana jest do niego wartość *256*, a flaga *CF* ustawiana jest na *1*, więc końcowa wartość AH wynosi *192*<br> -->

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