# AlictusCase

-Bütün küpler tek bir materyal kullanıyor ama texture'dan UV kaydırmak yerine renk setlediğim ve standart materyal kullandığım için draw callar biraz fazla oldu. Sahne ışıklandırmasını kapatarak biraz azalttım.

-Kendi kullandığım save/load sistemi var scriptable'lar için ama aktif değil. Zaten using UnityEditor kısımlarını #if UNITY_EDITOR içine almadığımız için build alamıyoruz, o yüzden öyle göstermelik bıraktım.

-Level seçmenin kolaylığı açısından level bitimine bir choose level kısmı ekledim (kaç tane level varsa o kadar gösteriyor)

-Level editör kısmında image istünden spawnlamayı yaptım, gayet de kolaymış. Ama bir editör window oluşturup tıkladığımızda obje spawnlansın, mausu takip edip gride snaplansın kısmını yapamadım. Aslında çözüme yakındım ama süreyi de çok aşmamak için öyle bırakıyorum. Script> CustomLevelEditor. Büyük ihtimal sizin buna kullandığınız fix bir çözüm vardır ve öğrenmek isterim.

-Fizik ayarlarını benzetmeye çabaladım ama sizdeki o smoothluğu yakalayamadım. Ayrıca player bir anda 180 derece dönerse bütün küpler etrafa saçılıyor, o yüzden dönme hızını düşük tuttum. Optimizasyon kısmı da dahil ne yaptığınızı merak ediyorum.
