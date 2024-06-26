Projenin iki temel bölümü var: Admin ve kullanıcı arayüzü.

Veri girişlerini Admin üzerinden yapıyoruz. 

Admin dosyaları  Siyasett.Web/Areas/Admin altında.

Resimler Siyasett.Web/wwwroot/images altında.

Ana sayfa ve menüler yapısı Siyasett.Web/Views/Shared/Components/_LayoutNew.cshtml'de düzenleniyor.

Projenin çok dilli yapısı veritabanındaki bir tabloya işleniyor. Kodda lokalize edilmiş yerler sql'de Posgres/Databases/Posgres/Schemas/Genel/Tables/Language_Resources'dan geliyor.

Kitaplar ve günlük siyaset yazıları ile ilgili tablolar şurada: Posgres/Databases/Posgres/Schemas/Yayinlar/Tables/


