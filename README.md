# 3D Rubik Küp Çözücü (Unity)

Bu proje **Unity** platformunda geliştirilmiş bir **3D Rubik Küp Çözücü** uygulamasıdır. Çözüm adımlarının bazılarında nadiren küçük hatalar görülebilse de genel olarak **yüksek başarı oranıyla** küp doğru şekilde çözülmektedir.

## Çözüm Algoritmaları

Proje, Rubik küpü **üst satırdan başlanarak** üç satır halinde sırasıyla çözmek üzerine tasarlanmıştır. Çözüm aşamaları aşağıda açıklanmıştır:

---

### 1. Üstte Beyaz Artı Oluşturma

- Eğer küp doğru pozisyonda ve yöndeyse işlem tamamlanır.
- Değilse, bir **Empty GameObject** kullanılarak kübün kenarları kontrol edilir. Beyaz yüzün hangi yöne baktığı belirlenir.
- İlk olarak **x** ekseni -1 yapılır (y fark etmeksizin).
- Beyaz yüzün konumu **(-1, 1, 0)** olacak şekilde ayarlanır.
- Üst satır, kübün yerleştirileceği kısım sağda kalacak şekilde döndürülür.
- Sağ sütun bir kez yukarı çevrilerek küp uygun konuma getirilir.

---

### 2. HAREKET4

- **Doğru pozisyonda olan küpler** (kırmızı yüzleri yukarı bakanlar) sayılır:
  - **4 küp doğruysa** işlem tamamdır.
  - **2 küp doğruysa:**
    - Karşılıklıysalar → birini ön yüze alıp algoritmayı **1 kez** çalıştır.
    - Komşuysalar → soldakini ön yüze alıp algoritmayı **2 kez** çalıştır.
  - **0 küp doğruysa:**
    - Rastgele birini ön yüze alıp algoritmayı **1 kez** çalıştır → ardından durum **2 komşuya** dönüşecektir → komşular algoritmasına geçilir.

---

### 3. HAREKET5

- **Üst orta parçalar** kontrol edilir:
  - Hepsi eşleşmişse → işlem tamamlanır.
  - Hiçbiri eşleşmemişse → algoritma çalıştırılır ve aşağıdaki işlemlere geçilir.
  - Eşleşenler karşılıklıysa → birini ön yüze alıp algoritmayı çalıştır → ardından alttaki işlem uygulanır.
  - Eşleşenler komşuysa → soldaki ön yüze alınır → algoritma çalıştırılır.

---

### 4. HAREKET6

- **Uyuşan köşe** var mı kontrol edilir:
  - Yoksa → rastgele bir pozisyondayken algoritma çalıştırılır → ardından başa dönülür.
  - Varsa → uyuşan köşe sağa alınır → 4 köşe oturana kadar algoritma tekrar edilir.
- **Son adım:** 4 köşe için **HAREKET1** algoritması tekrar uygulanır.

---

## Önemli Bilgilendirme

Projenin **doğru şekilde çalışabilmesi** için aşağıdaki klasörü indirip projenin ana dizinine eklemeniz gerekmektedir:

📂 **Gerekli Dosyalar:**  
[→ Dosyayı İndir](https://drive.google.com/drive/folders/15iPpTxV7x-MSgYt93GHXABTFbx3EUJyw?usp=sharing)

---

## Kurulum ve Çalıştırma

1. Bağlantıdaki klasörü projenin kök dizinine ekleyin.
2. Unity üzerinde projeyi açın ve çalıştırın.
3. Boşluk tuşuna bastığınızda rubik ilk önce karılacak ve ardından çözülecektir. Çözüm aşamalarında tamamlanan adımlar bildirilir.

---

## Not

Proje hâlâ geliştirme aşamasındadır. Nadiren de olsa çözüm algoritmalarında **3 adım civarında yanlışlıklar** oluşabilmektedir. İlerleyen sürümlerde bu hatalar minimize edilecektir.
