using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Siyasett.Data.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Active> Actives { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressType> AddressTypes { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AuthorizationLevel> AuthorizationLevels { get; set; }

    public virtual DbSet<ChronologiesParty> ChronologiesParties { get; set; }

    public virtual DbSet<Chronology> Chronologies { get; set; }

    public virtual DbSet<CommunicationType> CommunicationTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }

    public virtual DbSet<CompanyAttachment> CompanyAttachments { get; set; }

    public virtual DbSet<CompanyEmail> CompanyEmails { get; set; }

    public virtual DbSet<CompanyPhone> CompanyPhones { get; set; }

    public virtual DbSet<CompanySector> CompanySectors { get; set; }

    public virtual DbSet<CompanySocialMedia> CompanySocialMedias { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Convertcsv> Convertcsvs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<FaqGroup> FaqGroups { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GeneralLanguage> GeneralLanguages { get; set; }

    public virtual DbSet<Ilceler> Ilcelers { get; set; }

    public virtual DbSet<Iller> Illers { get; set; }

    public virtual DbSet<InstitutionType> InstitutionTypes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageResource> LanguageResources { get; set; }

    public virtual DbSet<ListType> ListTypes { get; set; }

    public virtual DbSet<MahalleSonuclar> MahalleSonuclars { get; set; }

    public virtual DbSet<MembershipUser> MembershipUsers { get; set; }

    public virtual DbSet<MshipType> MshipTypes { get; set; }

    public virtual DbSet<Party> Parties { get; set; }

    public virtual DbSet<PartyAddress> PartyAddresses { get; set; }

    public virtual DbSet<PartyAttachment> PartyAttachments { get; set; }

    public virtual DbSet<PartyEmail> PartyEmails { get; set; }

    public virtual DbSet<PartyIdeology> PartyIdeologies { get; set; }

    public virtual DbSet<PartyPhone> PartyPhones { get; set; }

    public virtual DbSet<PartySocialParty> PartySocialParties { get; set; }

    public virtual DbSet<PartyText> PartyTexts { get; set; }

    public virtual DbSet<PartyTextsSector> PartyTextsSectors { get; set; }

    public virtual DbSet<PeopleAddress> PeopleAddresses { get; set; }

    public virtual DbSet<PeopleAttachment> PeopleAttachments { get; set; }

    public virtual DbSet<PeopleEmail> PeopleEmails { get; set; }

    public virtual DbSet<PeopleJob> PeopleJobs { get; set; }

    public virtual DbSet<PeoplePhone> PeoplePhones { get; set; }

    public virtual DbSet<PeoplePosition> PeoplePositions { get; set; }

    public virtual DbSet<PeoplePositionSector> PeoplePositionSectors { get; set; }

    public virtual DbSet<PeopleSocialMedium> PeopleSocialMedia { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<PhoneType> PhoneTypes { get; set; }

    public virtual DbSet<PoliticalIdeology> PoliticalIdeologies { get; set; }

    public virtual DbSet<PoliticalPosition> PoliticalPositions { get; set; }

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<PollKind> PollKinds { get; set; }

    public virtual DbSet<PollResult> PollResults { get; set; }

    public virtual DbSet<PollType> PollTypes { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PositionField> PositionFields { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<SandikSonucListesi> SandikSonucListesis { get; set; }

    public virtual DbSet<SandikSonucTutanaklari> SandikSonucTutanaklaris { get; set; }

    public virtual DbSet<Secim> Secims { get; set; }

    public virtual DbSet<SecimDetay> SecimDetays { get; set; }

    public virtual DbSet<SecimDetayListesi> SecimDetayListesis { get; set; }

    public virtual DbSet<SecimIlListesi> SecimIlListesis { get; set; }

    public virtual DbSet<SecimIlceListesi> SecimIlceListesis { get; set; }

    public virtual DbSet<SecimIlceler> SecimIlcelers { get; set; }

    public virtual DbSet<SecimIller> SecimIllers { get; set; }

    public virtual DbSet<SecimIttifaklar> SecimIttifaklars { get; set; }

    public virtual DbSet<SecimMahalleListesi> SecimMahalleListesis { get; set; }

    public virtual DbSet<SecimMahalleSecimBilgisi> SecimMahalleSecimBilgisis { get; set; }

    public virtual DbSet<SecimMuhtarlikListe> SecimMuhtarlikListes { get; set; }

    public virtual DbSet<SecimMuhtarlikOzet> SecimMuhtarlikOzets { get; set; }

    public virtual DbSet<SecimMuhtarlikSandiklar> SecimMuhtarlikSandiklars { get; set; }

    public virtual DbSet<SecimMuhtarliklar> SecimMuhtarliklars { get; set; }

    public virtual DbSet<SecimPartiler> SecimPartilers { get; set; }

    public virtual DbSet<SecimSandikSonuc> SecimSandikSonucs { get; set; }

    public virtual DbSet<SecimSandikSonucList> SecimSandikSonucLists { get; set; }

    public virtual DbSet<SecimSonucBasliklari> SecimSonucBasliklaris { get; set; }

    public virtual DbSet<SecimSonucListesi> SecimSonucListesis { get; set; }

    public virtual DbSet<SecimSonuclar> SecimSonuclars { get; set; }

    public virtual DbSet<SecimTurleri> SecimTurleris { get; set; }

    public virtual DbSet<Secimler> Secimlers { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<SessionCache> SessionCaches { get; set; }

    public virtual DbSet<SocialMedium> SocialMedia { get; set; }

    public virtual DbSet<SocialType> SocialTypes { get; set; }

    public virtual DbSet<Sonuclar> Sonuclars { get; set; }

    public virtual DbSet<VideoLibrary> VideoLibraries { get; set; }

    public virtual DbSet<VideoSession> VideoSessions { get; set; }

    public virtual DbSet<YenilemeSecimList> YenilemeSecimLists { get; set; }

    public virtual DbSet<YynDiller> YynDillers { get; set; }

    public virtual DbSet<YynKategoriler> YynKategorilers { get; set; }

    public virtual DbSet<YynKitapKategoriler> YynKitapKategorilers { get; set; }

    public virtual DbSet<YynKitapYazar> YynKitapYazars { get; set; }

    public virtual DbSet<YynKitaplar> YynKitaplars { get; set; }

    public virtual DbSet<YynKoseYazilari> YynKoseYazilaris { get; set; }

    public virtual DbSet<YynKoseYazilariKategorileri> YynKoseYazilariKategorileris { get; set; }

    public virtual DbSet<YynTurler> YynTurlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=siyasett-pgsql.postgres.database.azure.com;Port=5432;Database=postgres;User Id=systadmin;Password=ST*23.!?pg;Enlist=false", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Active>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("actives_pk");

            entity.ToTable("actives", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pk");

            entity.ToTable("addresses", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .HasColumnName("address2");
            entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");

            entity.HasOne(d => d.AddressType).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AddressTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_fk");

            entity.HasOne(d => d.Country).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_country_fk");
        });

        modelBuilder.Entity<AddressType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_types_pk");

            entity.ToTable("address_types", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.addresses_types_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.ToTable("AspNetRoles", "users");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.ToTable("AspNetRoleClaims", "users");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.ToTable("AspNetUsers", "users");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.UserTypeId).HasDefaultValueSql("1");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles", "users");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.ToTable("AspNetUserClaims", "users");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.ToTable("AspNetUserLogins", "users");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.ToTable("AspNetUserTokens", "users");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attachments_pk");

            entity.ToTable("attachments", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .HasColumnName("content_type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .HasColumnName("file_name");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(255)
                .HasColumnName("original_file_name");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<AuthorizationLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("authorization_levels_pk");

            entity.ToTable("authorization_levels", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<ChronologiesParty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chronologies_party_pk");

            entity.ToTable("chronologies_party", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Chronologyid).HasColumnName("chronologyid");
            entity.Property(e => e.Partyid).HasColumnName("partyid");

            entity.HasOne(d => d.Chronology).WithMany(p => p.ChronologiesParties)
                .HasForeignKey(d => d.Chronologyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chronologies_party_fk_1");
        });

        modelBuilder.Entity<Chronology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chronologies_pk");

            entity.ToTable("chronologies", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.DescriptionTr).HasColumnName("description_tr");
            entity.Property(e => e.EventDate).HasColumnName("event_date");
            entity.Property(e => e.KeywordsEn)
                .HasMaxLength(1000)
                .HasColumnName("keywords_en");
            entity.Property(e => e.KeywordsTr)
                .HasMaxLength(1000)
                .HasColumnName("keywords_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<CommunicationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comunication_types_pk");

            entity.ToTable("communication_types", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.comunication_types_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(50)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(50)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companies_pk");

            entity.ToTable("companies", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CompanyLeaderId).HasColumnName("company_leader_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Dof)
                .HasMaxLength(20)
                .HasComment("kuruluş tarihi")
                .HasColumnName("dof");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ShortName)
                .HasColumnType("character varying")
                .HasColumnName("short_name");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.WebAdress)
                .HasColumnType("character varying")
                .HasColumnName("web_adress");
        });

        modelBuilder.Entity<CompanyAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_addresses_pk");

            entity.ToTable("company_addresses", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            entity.HasOne(d => d.Address).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_addresses_fk");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAddresses)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_addresses_fk_1");
        });

        modelBuilder.Entity<CompanyAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_attacments_pk_1");

            entity.ToTable("company_attachments", "genel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.company_attacments_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");

            entity.HasOne(d => d.Attachment).WithMany(p => p.CompanyAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_attacments_fk");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyAttachments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_attacments_fk_1");
        });

        modelBuilder.Entity<CompanyEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_emails_pk_1");

            entity.ToTable("company_emails", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.EmailId).HasColumnName("email_id");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyEmails)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_emails_fk_1");

            entity.HasOne(d => d.Email).WithMany(p => p.CompanyEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_emails_fk");
        });

        modelBuilder.Entity<CompanyPhone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_phones_pk_1");

            entity.ToTable("company_phones", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.PhoneId).HasColumnName("phone_id");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyPhones)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_phones_fk");

            entity.HasOne(d => d.Phone).WithMany(p => p.CompanyPhones)
                .HasForeignKey(d => d.PhoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_phones_fk_1");
        });

        modelBuilder.Entity<CompanySector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_sectors_pk");

            entity.ToTable("company_sectors", "genel");

            entity.HasIndex(e => e.Id, "company_sectors_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.SectorId).HasColumnName("sector_id");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanySectors)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_sectors_fk_1");

            entity.HasOne(d => d.Sector).WithMany(p => p.CompanySectors)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_sectors_fk");
        });

        modelBuilder.Entity<CompanySocialMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id_pk_1");

            entity.ToTable("company_social_medias", "genel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.id_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.SocialMediaId).HasColumnName("social_media_id");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanySocialMedia)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_social_medias_fk_1");

            entity.HasOne(d => d.SocialMedia).WithMany(p => p.CompanySocialMedia)
                .HasForeignKey(d => d.SocialMediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_social_medias_fk");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contacts_pk");

            entity.ToTable("contacts", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Message)
                .HasColumnType("character varying")
                .HasColumnName("message");
            entity.Property(e => e.MessageDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("message_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Subject)
                .HasColumnType("character varying")
                .HasColumnName("subject");
        });

        modelBuilder.Entity<Convertcsv>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("convertcsv", "genel");

            entity.Property(e => e.AskiBaslangicTarihi)
                .HasMaxLength(50)
                .HasColumnName("aski_BASLANGIC_TARIHI");
            entity.Property(e => e.AskiBitisTarihi)
                .HasMaxLength(50)
                .HasColumnName("aski_BITIS_TARIHI");
            entity.Property(e => e.BirlestirmeSayi).HasColumnName("birlestirme_SAYI");
            entity.Property(e => e.GumrukSayi).HasColumnName("gumruk_SAYI");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruAdi)
                .HasMaxLength(50)
                .HasColumnName("secim_TURU_ADI");
            entity.Property(e => e.SecimTuruKodu).HasColumnName("secim_TURU_KODU");
            entity.Property(e => e.TutanakSayi).HasColumnName("tutanak_SAYI");
            entity.Property(e => e.YurtdisiSayi).HasColumnName("yurtdisi_SAYI");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("countriest_pk");

            entity.ToTable("countries", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.countries_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.Iso2)
                .HasMaxLength(2)
                .HasColumnName("iso2");
            entity.Property(e => e.Iso3)
                .HasMaxLength(4)
                .HasColumnName("iso3");
            entity.Property(e => e.NameEn)
                .HasMaxLength(50)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(50)
                .HasColumnName("name_tr");
            entity.Property(e => e.PhoneCode)
                .HasMaxLength(32)
                .HasColumnName("phone_code");
            entity.Property(e => e.Sira).HasColumnName("sira");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currencies_pk");

            entity.ToTable("currencies", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbbrEn)
                .HasMaxLength(10)
                .HasColumnName("abbr_en");
            entity.Property(e => e.AbbrTr)
                .HasMaxLength(10)
                .HasColumnName("abbr_tr");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdtime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdtime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.Symbol)
                .HasMaxLength(10)
                .HasColumnName("symbol");
            entity.Property(e => e.Updatedby).HasColumnName("updatedby");
            entity.Property(e => e.Updatedtime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedtime");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("districts_pk");

            entity.ToTable("districts", "combos");

            entity.HasIndex(e => e.ProvinceId, "districts_province_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(MultiPolygon,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("education_pk");

            entity.ToTable("educations", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.education_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("emails_pk");

            entity.ToTable("emails", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommunicationTypeId).HasColumnName("communication_type_id");
            entity.Property(e => e.EmailAddress)
                .HasColumnType("character varying")
                .HasColumnName("email_address");

            entity.HasOne(d => d.CommunicationType).WithMany(p => p.Emails)
                .HasForeignKey(d => d.CommunicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("emails_fk");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("faq_pk");

            entity.ToTable("faq", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnswerEn).HasColumnName("answer_en");
            entity.Property(e => e.AnswerTr).HasColumnName("answer_tr");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.FaqGroupId).HasColumnName("faq_group_id");
            entity.Property(e => e.QuestionEn).HasColumnName("question_en");
            entity.Property(e => e.QuestionTr).HasColumnName("question_tr");

            entity.HasOne(d => d.FaqGroup).WithMany(p => p.Faqs)
                .HasForeignKey(d => d.FaqGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("faq_fk");
        });

        modelBuilder.Entity<FaqGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("faq_groups_pk");

            entity.ToTable("faq_groups", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameEn)
                .HasMaxLength(250)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(250)
                .HasColumnName("name_tr");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feedback_pk");

            entity.ToTable("feedback", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer)
                .HasColumnType("character varying")
                .HasColumnName("answer");
            entity.Property(e => e.AnswerTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("answer_time");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");
            entity.Property(e => e.Message)
                .HasColumnType("character varying")
                .HasColumnName("message");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PageLink)
                .HasColumnType("character varying")
                .HasColumnName("page_link");
            entity.Property(e => e.SendTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("send_time");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Topic)
                .HasColumnType("character varying")
                .HasColumnName("topic");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genders_pk");

            entity.ToTable("genders", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<GeneralLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("general_languages_pk");

            entity.ToTable("general_language", "combos");

            entity.HasIndex(e => e.Id, "general_languages_id_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.general_languages_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Nameen)
                .HasColumnType("character varying")
                .HasColumnName("nameen");
            entity.Property(e => e.Nametr)
                .HasColumnType("character varying")
                .HasColumnName("nametr");
        });

        modelBuilder.Entity<Ilceler>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("ilceler_pk");

            entity.ToTable("ilceler", "genel");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.BeldeId).HasColumnName("belde_ID");
            entity.Property(e => e.BirimId).HasColumnName("birim_ID");
            entity.Property(e => e.Id)
                .HasMaxLength(30)
                .HasColumnName("id");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(100)
                .HasColumnName("il_ADI");
            entity.Property(e => e.IlId).HasColumnName("il_ID");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(100)
                .HasColumnName("ilce_ADI");
            entity.Property(e => e.IlceId).HasColumnName("ilce_ID");
            entity.Property(e => e.Isok)
                .HasDefaultValueSql("false")
                .HasColumnName("isok");
            entity.Property(e => e.SecilecekAdaySayisi).HasColumnName("secilecek_ADAY_SAYISI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_CEVRESI_ID");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.YerlesimYeriTuru).HasColumnName("yerlesim_YERI_TURU");
        });

        modelBuilder.Entity<Iller>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("iller_pk");

            entity.ToTable("iller", "genel");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.Id)
                .HasMaxLength(30)
                .HasColumnName("id");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(250)
                .HasColumnName("il_ADI");
            entity.Property(e => e.IlId).HasColumnName("il_ID");
            entity.Property(e => e.SecilecekAdaySayisi).HasColumnName("secilecek_ADAY_SAYISI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_CEVRESI_ID");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.YerlesimYeriTuru).HasColumnName("yerlesim_YERI_TURU");
        });

        modelBuilder.Entity<InstitutionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("institution_types_pk");

            entity.ToTable("institution_types", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(50)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(50)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobs_pk");

            entity.ToTable("jobs", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("languages_pk");

            entity.ToTable("languages", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Culture)
                .HasMaxLength(20)
                .HasColumnName("culture");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<LanguageResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("language_resources_pk");

            entity.ToTable("language_resources", "genel");

            entity.HasIndex(e => new { e.LanguageId, e.Name }, "language_resources_language_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Language).WithMany(p => p.LanguageResources)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("language_resources_fk");
        });

        modelBuilder.Entity<ListType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_types_pk");

            entity.ToTable("list_types", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<MahalleSonuclar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mahalle_sonuclar", "election");

            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.OySayisi).HasColumnName("oy_sayisi");
            entity.Property(e => e.PartiId).HasColumnName("parti_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimIlceListesiId).HasColumnName("secim_ilce_listesi_id");
            entity.Property(e => e.SecimIlcelerId).HasColumnName("secim_ilceler_id");
            entity.Property(e => e.SecimIllerId).HasColumnName("secim_iller_id");
            entity.Property(e => e.SecimMuhtarliklarId).HasColumnName("secim_muhtarliklar_id");
            entity.Property(e => e.YskSecimCevresiId).HasColumnName("ysk_secim_cevresi_id");
        });

        modelBuilder.Entity<MembershipUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("membership_user_pk");

            entity.ToTable("membership_user", "users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MshipEnddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("mship_enddate");
            entity.Property(e => e.MshipStartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("mship_startdate");
            entity.Property(e => e.MshipType).HasColumnName("mship_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<MshipType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("mship_types", "combos");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.MshiptypeName)
                .HasColumnType("character varying")
                .HasColumnName("mshiptype_name");
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_pk");

            entity.ToTable("parties", "genel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.party_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Closed).HasColumnName("closed");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Dof)
                .HasMaxLength(20)
                .HasComment("kuruluş tarihi")
                .HasColumnName("dof");
            entity.Property(e => e.LeaderPeopleId).HasColumnName("leader_people_id");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .HasColumnName("logo");
            entity.Property(e => e.MemberCount)
                .HasDefaultValueSql("0")
                .HasColumnName("member_count");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NameEn)
                .HasColumnType("character varying")
                .HasColumnName("name_en");
            entity.Property(e => e.Parliamenteries).HasColumnName("parliamenteries");
            entity.Property(e => e.ParticipateElection).HasColumnName("participate_election");
            entity.Property(e => e.PoliticalPositionId).HasColumnName("political_position_id");
            entity.Property(e => e.ShortName)
                .HasMaxLength(100)
                .HasColumnName("short_name");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.WebSite)
                .HasMaxLength(100)
                .HasColumnName("web_site");
            entity.Property(e => e.YskId).HasColumnName("ysk_id");

            entity.HasOne(d => d.LeaderPeople).WithMany(p => p.Parties)
                .HasForeignKey(d => d.LeaderPeopleId)
                .HasConstraintName("parties_fk");

            entity.HasOne(d => d.PoliticalPosition).WithMany(p => p.Parties)
                .HasForeignKey(d => d.PoliticalPositionId)
                .HasConstraintName("parties_politicalpos_fk");
        });

        modelBuilder.Entity<PartyAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_addresses_pk");

            entity.ToTable("party_addresses", "genel");

            entity.HasIndex(e => new { e.PartyId, e.AddressId }, "party_addresses_party_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");

            entity.HasOne(d => d.Address).WithMany(p => p.PartyAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_addresses_address_fk");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyAddresses)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_addresses_fk");
        });

        modelBuilder.Entity<PartyAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_attacments_pk");

            entity.ToTable("party_attachments", "genel");

            entity.HasIndex(e => new { e.PartyId, e.AttachmentId }, "party_attacments_party_id_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.party_attacments_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");

            entity.HasOne(d => d.Attachment).WithMany(p => p.PartyAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_attacments_fk_1");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyAttachments)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_attacments_fk");
        });

        modelBuilder.Entity<PartyEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_emails_pk");

            entity.ToTable("party_emails", "genel");

            entity.HasIndex(e => new { e.EmailId, e.PartyId }, "party_emails_email_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");

            entity.HasOne(d => d.Email).WithMany(p => p.PartyEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_emails_fk");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyEmails)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_emails_fk_1");
        });

        modelBuilder.Entity<PartyIdeology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_ideology_pk");

            entity.ToTable("party_ideologies", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.PoliticialIdeologyId).HasColumnName("politicial_ideology_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyIdeologies)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_ideologiesparties_fk");

            entity.HasOne(d => d.PoliticialIdeology).WithMany(p => p.PartyIdeologies)
                .HasForeignKey(d => d.PoliticialIdeologyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_ideologiespolideo_fk");
        });

        modelBuilder.Entity<PartyPhone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_phones_pk");

            entity.ToTable("party_phones", "genel");

            entity.HasIndex(e => new { e.PhoneId, e.PartyId }, "party_phones_phone_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.PhoneId).HasColumnName("phone_id");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyPhones)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_phones_fk");

            entity.HasOne(d => d.Phone).WithMany(p => p.PartyPhones)
                .HasForeignKey(d => d.PhoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_phones_fk_1");
        });

        modelBuilder.Entity<PartySocialParty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("id_pk");

            entity.ToTable("party_social_parties", "genel");

            entity.HasIndex(e => new { e.PartyId, e.SocialMediaId }, "id_party_id_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.id_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.SocialMediaId).HasColumnName("social_media_id");

            entity.HasOne(d => d.Party).WithMany(p => p.PartySocialParties)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_fk");

            entity.HasOne(d => d.SocialMedia).WithMany(p => p.PartySocialParties)
                .HasForeignKey(d => d.SocialMediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_fk_1");
        });

        modelBuilder.Entity<PartyText>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_texts_pk");

            entity.ToTable("party_texts", "genel");

            entity.HasIndex(e => e.Partyid, "party_texts_partyid_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Partyid).HasColumnName("partyid");
            entity.Property(e => e.SectorId)
                .HasDefaultValueSql("1")
                .HasColumnName("sector_id");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.Party).WithMany(p => p.PartyTexts)
                .HasForeignKey(d => d.Partyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_texts_fk");

            entity.HasOne(d => d.Sector).WithMany(p => p.PartyTexts)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("party_texts_sector_fk");
        });

        modelBuilder.Entity<PartyTextsSector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("party_texts_sectors_pk");

            entity.ToTable("party_texts_sectors", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartyTextid).HasColumnName("party_textid");
            entity.Property(e => e.Sectorid).HasColumnName("sectorid");
        });

        modelBuilder.Entity<PeopleAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_addresses_pk");

            entity.ToTable("people_addresses", "genel");

            entity.HasIndex(e => new { e.PeopleId, e.AddressId }, "person_addresses_people_id_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.person_addresses_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");

            entity.HasOne(d => d.Address).WithMany(p => p.PeopleAddresses)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("person_addresses_fk_1");

            entity.HasOne(d => d.People).WithMany(p => p.PeopleAddresses)
                .HasForeignKey(d => d.PeopleId)
                .HasConstraintName("person_addresses_fk");
        });

        modelBuilder.Entity<PeopleAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_attachments_pk");

            entity.ToTable("people_attachments", "genel");

            entity.HasIndex(e => new { e.PeopleId, e.AttachmentId }, "people_attachments_people_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");

            entity.HasOne(d => d.Attachment).WithMany(p => p.PeopleAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_attachments_fk");

            entity.HasOne(d => d.People).WithMany(p => p.PeopleAttachments)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_attachments_fk_1");
        });

        modelBuilder.Entity<PeopleEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_emails_pk");

            entity.ToTable("people_emails", "genel");

            entity.HasIndex(e => new { e.PeopleId, e.EmailId }, "people_emails_people_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");

            entity.HasOne(d => d.Email).WithMany(p => p.PeopleEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_emails_fk_1");

            entity.HasOne(d => d.People).WithMany(p => p.PeopleEmails)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_emails_fk");
        });

        modelBuilder.Entity<PeopleJob>(entity =>
        {
            entity.HasKey(e => new { e.PeopleId, e.JobId }).HasName("people_jobs_pk");

            entity.ToTable("people_jobs", "genel");

            entity.HasIndex(e => e.Id, "people_jobs_id_idx").IsUnique();

            entity.Property(e => e.PeopleId).HasColumnName("people_id");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(d => d.Job).WithMany(p => p.PeopleJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_jobs_fk_1");

            entity.HasOne(d => d.People).WithMany(p => p.PeopleJobs)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_jobs_fk");
        });

        modelBuilder.Entity<PeoplePhone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_phones_pk");

            entity.ToTable("people_phones", "genel");

            entity.HasIndex(e => new { e.PhoneId, e.PeopleId }, "person_phones_phone_id_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.person_phones_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");
            entity.Property(e => e.PhoneId).HasColumnName("phone_id");

            entity.HasOne(d => d.People).WithMany(p => p.PeoplePhones)
                .HasForeignKey(d => d.PeopleId)
                .HasConstraintName("person_phones_fk_1");

            entity.HasOne(d => d.Phone).WithMany(p => p.PeoplePhones)
                .HasForeignKey(d => d.PhoneId)
                .HasConstraintName("person_phones_fk");
        });

        modelBuilder.Entity<PeoplePosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_positions_new_pk");

            entity.ToTable("people_positions", "genel");

            entity.HasIndex(e => e.PeopleId, "people_positions_new_people_id_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.people_positions2_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EndDay).HasColumnName("end_day");
            entity.Property(e => e.EndMonth).HasColumnName("end_month");
            entity.Property(e => e.EndYear).HasColumnName("end_year");
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(250)
                .HasColumnName("institution_name");
            entity.Property(e => e.InstitutionTypeId).HasColumnName("institution_type_id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");
            entity.Property(e => e.Period)
                .HasMaxLength(20)
                .HasColumnName("period");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.SectorId).HasColumnName("sector_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartDay).HasColumnName("start_day");
            entity.Property(e => e.StartMonth).HasColumnName("start_month");
            entity.Property(e => e.StartYear).HasColumnName("start_year");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.InstitutionType).WithMany(p => p.PeoplePositions)
                .HasForeignKey(d => d.InstitutionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_positions_fk_2");

            entity.HasOne(d => d.Party).WithMany(p => p.PeoplePositions)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("people_positions_fk");

            entity.HasOne(d => d.People).WithMany(p => p.PeoplePositions)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_positions_fk_1");

            entity.HasOne(d => d.Position).WithMany(p => p.PeoplePositions)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_positions_positions_fk");

            entity.HasOne(d => d.Sector).WithMany(p => p.PeoplePositions)
                .HasForeignKey(d => d.SectorId)
                .HasConstraintName("people_positions_sectors_fk");
        });

        modelBuilder.Entity<PeoplePositionSector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_position_sectors_pk");

            entity.ToTable("people_position_sectors", "genel");

            entity.HasIndex(e => new { e.PeoplePositionId, e.SectorId }, "people_position_sectors_people_position_id_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PeoplePositionId).HasColumnName("people_position_id");
            entity.Property(e => e.SectorId).HasColumnName("sector_id");

            entity.HasOne(d => d.PeoplePosition).WithMany(p => p.PeoplePositionSectors)
                .HasForeignKey(d => d.PeoplePositionId)
                .HasConstraintName("people_position_sectors_fk");

            entity.HasOne(d => d.Sector).WithMany(p => p.PeoplePositionSectors)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_position_sectors_fk_1");
        });

        modelBuilder.Entity<PeopleSocialMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_socialmedia_pk");

            entity.ToTable("people_social_media", "genel");

            entity.HasIndex(e => new { e.PeopleId, e.SocialMediaId }, "people_social_media_people_id_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.people_socialmedia_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PeopleId).HasColumnName("people_id");
            entity.Property(e => e.SocialMediaId).HasColumnName("social_media_id");

            entity.HasOne(d => d.People).WithMany(p => p.PeopleSocialMedia)
                .HasForeignKey(d => d.PeopleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_social_media_fk");

            entity.HasOne(d => d.SocialMedia).WithMany(p => p.PeopleSocialMedia)
                .HasForeignKey(d => d.SocialMediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_social_media_fk_1");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_pk");

            entity.ToTable("people", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.CvEn).HasColumnName("cv_en");
            entity.Property(e => e.CvTr).HasColumnName("cv_tr");
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(50)
                .HasColumnName("date_of_birth");
            entity.Property(e => e.EducationId).HasColumnName("education_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.FirstNameNormalize)
                .HasMaxLength(50)
                .HasColumnName("first_name_normalize");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.InstitutionTypeId).HasColumnName("institution_type_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.LastNameNormalize)
                .HasMaxLength(50)
                .HasColumnName("last_name_normalize");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.Photo)
                .HasMaxLength(50)
                .HasColumnName("photo");
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(100)
                .HasColumnName("place_of_birth");
            entity.Property(e => e.PositionFieldId).HasColumnName("position_field_id");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Views).HasColumnName("views");

            entity.HasOne(d => d.Education).WithMany(p => p.People)
                .HasForeignKey(d => d.EducationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_education_fk");

            entity.HasOne(d => d.Gender).WithMany(p => p.People)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_gender_fk");

            entity.HasOne(d => d.InstitutionType).WithMany(p => p.People)
                .HasForeignKey(d => d.InstitutionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_institution_type_fk");

            entity.HasOne(d => d.PositionField).WithMany(p => p.People)
                .HasForeignKey(d => d.PositionFieldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("people_positionfield_fk");

            entity.HasOne(d => d.User).WithMany(p => p.People)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("people_users_fk");
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phones_pk");

            entity.ToTable("phones", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommunicationTypeId)
                .HasDefaultValueSql("1")
                .HasColumnName("communication_type_id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(20)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryId)
                .HasDefaultValueSql("220")
                .HasColumnName("country_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.PhoneTypeId).HasColumnName("phone_type_id");
            entity.Property(e => e.PrefixCode)
                .HasMaxLength(10)
                .HasColumnName("prefix_code");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");

            entity.HasOne(d => d.CommunicationType).WithMany(p => p.Phones)
                .HasForeignKey(d => d.CommunicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phones_communication_fk");

            entity.HasOne(d => d.Country).WithMany(p => p.Phones)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phones_country_fk");

            entity.HasOne(d => d.PhoneType).WithMany(p => p.Phones)
                .HasForeignKey(d => d.PhoneTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phones_fk");
        });

        modelBuilder.Entity<PhoneType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phone_types_pk");

            entity.ToTable("phone_types", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<PoliticalIdeology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("political_idologies_pk");

            entity.ToTable("political_ideologies", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.political_idologies_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
        });

        modelBuilder.Entity<PoliticalPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("political_positions_pk");

            entity.ToTable("political_positions", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
        });

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("poll_pk");

            entity.ToTable("poll", "genel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.PollCompanyId).HasColumnName("poll_company_id");
            entity.Property(e => e.PollKindId).HasColumnName("poll_kind_id");
            entity.Property(e => e.PollTypeId).HasColumnName("poll_type_id");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.PollCompany).WithMany(p => p.Polls)
                .HasForeignKey(d => d.PollCompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll_fk2");

            entity.HasOne(d => d.PollKind).WithMany(p => p.Polls)
                .HasForeignKey(d => d.PollKindId)
                .HasConstraintName("poll_fk");

            entity.HasOne(d => d.PollType).WithMany(p => p.Polls)
                .HasForeignKey(d => d.PollTypeId)
                .HasConstraintName("poll_fk_1");
        });

        modelBuilder.Entity<PollKind>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("poll_kind_pk");

            entity.ToTable("poll_kind", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.education_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<PollResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("poll_results_pk");

            entity.ToTable("poll_results", "genel");

            entity.HasIndex(e => e.PollId, "poll_results_poll_id_idx");

            entity.HasIndex(e => new { e.PollId, e.PartyId }, "poll_results_poll_id_party_idx").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.poll_parties_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.PartyId).HasColumnName("party_id");
            entity.Property(e => e.PollId).HasColumnName("poll_id");
            entity.Property(e => e.Ratio).HasColumnName("ratio");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");

            entity.HasOne(d => d.Party).WithMany(p => p.PollResults)
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll_results_fk");

            entity.HasOne(d => d.Poll).WithMany(p => p.PollResults)
                .HasForeignKey(d => d.PollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("poll_results_fk_1");
        });

        modelBuilder.Entity<PollType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("poll_types_pk");

            entity.ToTable("poll_types", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("positions_pk");

            entity.ToTable("positions", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.Weightiness).HasColumnName("weightiness");
        });

        modelBuilder.Entity<PositionField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("position_fields_pk");

            entity.ToTable("position_fields", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provinces_pk");

            entity.ToTable("provinces", "combos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CountryId)
                .HasDefaultValueSql("182")
                .HasColumnName("country_id");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(MultiPolygon,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.Name)
                .HasMaxLength(14)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("regions_pk");

            entity.ToTable("regions", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        });

        modelBuilder.Entity<SandikSonucListesi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sandik_sonuc_listesi", "genel");

            entity.Property(e => e.Bagimsiz10AldigiOy).HasColumnName("bagimsiz10_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz11AldigiOy).HasColumnName("bagimsiz11_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz12AldigiOy).HasColumnName("bagimsiz12_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz13AldigiOy).HasColumnName("bagimsiz13_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz14AldigiOy).HasColumnName("bagimsiz14_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz15AldigiOy).HasColumnName("bagimsiz15_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz16AldigiOy).HasColumnName("bagimsiz16_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz17AldigiOy).HasColumnName("bagimsiz17_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz18AldigiOy).HasColumnName("bagimsiz18_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz19AldigiOy).HasColumnName("bagimsiz19_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz1AldigiOy).HasColumnName("bagimsiz1_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz20AldigiOy).HasColumnName("bagimsiz20_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz21AldigiOy).HasColumnName("bagimsiz21_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz22AldigiOy).HasColumnName("bagimsiz22_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz23AldigiOy).HasColumnName("bagimsiz23_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz24AldigiOy).HasColumnName("bagimsiz24_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz25AldigiOy).HasColumnName("bagimsiz25_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz26AldigiOy).HasColumnName("bagimsiz26_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz27AldigiOy).HasColumnName("bagimsiz27_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz28AldigiOy).HasColumnName("bagimsiz28_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz29AldigiOy).HasColumnName("bagimsiz29_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz2AldigiOy).HasColumnName("bagimsiz2_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz30AldigiOy).HasColumnName("bagimsiz30_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz31AldigiOy).HasColumnName("bagimsiz31_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz32AldigiOy).HasColumnName("bagimsiz32_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz33AldigiOy).HasColumnName("bagimsiz33_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz34AldigiOy).HasColumnName("bagimsiz34_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz35AldigiOy).HasColumnName("bagimsiz35_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz36AldigiOy).HasColumnName("bagimsiz36_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz37AldigiOy).HasColumnName("bagimsiz37_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz38AldigiOy).HasColumnName("bagimsiz38_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz39AldigiOy).HasColumnName("bagimsiz39_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz3AldigiOy).HasColumnName("bagimsiz3_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz40AldigiOy).HasColumnName("bagimsiz40_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz41AldigiOy).HasColumnName("bagimsiz41_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz42AldigiOy).HasColumnName("bagimsiz42_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz43AldigiOy).HasColumnName("bagimsiz43_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz44AldigiOy).HasColumnName("bagimsiz44_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz45AldigiOy).HasColumnName("bagimsiz45_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz46AldigiOy).HasColumnName("bagimsiz46_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz47AldigiOy).HasColumnName("bagimsiz47_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz48AldigiOy).HasColumnName("bagimsiz48_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz49AldigiOy).HasColumnName("bagimsiz49_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz4AldigiOy).HasColumnName("bagimsiz4_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz50AldigiOy).HasColumnName("bagimsiz50_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz5AldigiOy).HasColumnName("bagimsiz5_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz6AldigiOy).HasColumnName("bagimsiz6_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz7AldigiOy).HasColumnName("bagimsiz7_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz8AldigiOy).HasColumnName("bagimsiz8_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz9AldigiOy).HasColumnName("bagimsiz9_ALDIGI_OY");
            entity.Property(e => e.BagimsizToplamOy).HasColumnName("bagimsiz_TOPLAM_OY");
            entity.Property(e => e.BeldeId).HasColumnName("belde_ID");
            entity.Property(e => e.BirimId).HasColumnName("birim_ID");
            entity.Property(e => e.BirlestirmeTutanagiDisTemsilcilik).HasColumnName("birlestirme_TUTANAGI_DIS_TEMSILCILIK");
            entity.Property(e => e.BirlestirmeTutanagiGumrukIlce).HasColumnName("birlestirme_TUTANAGI_GUMRUK_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiGumrukKurul).HasColumnName("birlestirme_TUTANAGI_GUMRUK_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiGumrukRumuz).HasColumnName("birlestirme_TUTANAGI_GUMRUK_RUMUZ");
            entity.Property(e => e.BirlestirmeTutanagiGumrukler).HasColumnName("birlestirme_TUTANAGI_GUMRUKLER");
            entity.Property(e => e.BirlestirmeTutanagiIl).HasColumnName("birlestirme_TUTANAGI_IL");
            entity.Property(e => e.BirlestirmeTutanagiIlce).HasColumnName("birlestirme_TUTANAGI_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiKurul).HasColumnName("birlestirme_TUTANAGI_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiTumdunya).HasColumnName("birlestirme_TUTANAGI_TUMDUNYA");
            entity.Property(e => e.BirlestirmeTutanagiUlke).HasColumnName("birlestirme_TUTANAGI_ULKE");
            entity.Property(e => e.BirlestirmeTutanagiUlkeler).HasColumnName("birlestirme_TUTANAGI_ULKELER");
            entity.Property(e => e.CezaeviId).HasColumnName("cezaevi_ID");
            entity.Property(e => e.DegisiklikNedeni).HasColumnName("degisiklik_NEDENI");
            entity.Property(e => e.DenetimTutanagi).HasColumnName("denetim_TUTANAGI");
            entity.Property(e => e.DisTemsilcilikId).HasColumnName("dis_TEMSILCILIK_ID");
            entity.Property(e => e.GecerliOyToplami).HasColumnName("gecerli_OY_TOPLAMI");
            entity.Property(e => e.GecersizOyToplami).HasColumnName("gecersiz_OY_TOPLAMI");
            entity.Property(e => e.GumrukId).HasColumnName("gumruk_ID");
            entity.Property(e => e.GumrukSandikTarihi)
                .HasMaxLength(50)
                .HasColumnName("gumruk_SANDIK_TARIHI");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(50)
                .HasColumnName("il_ADI");
            entity.Property(e => e.IlId).HasColumnName("il_ID");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(50)
                .HasColumnName("ilce_ADI");
            entity.Property(e => e.IlceId).HasColumnName("ilce_ID");
            entity.Property(e => e.ItirazliGecerliOySayisi).HasColumnName("itirazli_GECERLI_OY_SAYISI");
            entity.Property(e => e.ItirazsizGecerliOySayisi).HasColumnName("itirazsiz_GECERLI_OY_SAYISI");
            entity.Property(e => e.Ittifak1AldigiOy).HasColumnName("ittifak1_ALDIGI_OY");
            entity.Property(e => e.Ittifak2AldigiOy).HasColumnName("ittifak2_ALDIGI_OY");
            entity.Property(e => e.Ittifak3AldigiOy).HasColumnName("ittifak3_ALDIGI_OY");
            entity.Property(e => e.Ittifak4AldigiOy).HasColumnName("ittifak4_ALDIGI_OY");
            entity.Property(e => e.Ittifak5AldigiOy).HasColumnName("ittifak5_ALDIGI_OY");
            entity.Property(e => e.MuhtarlikAdi)
                .HasMaxLength(50)
                .HasColumnName("muhtarlik_ADI");
            entity.Property(e => e.MuhtarlikId).HasColumnName("muhtarlik_ID");
            entity.Property(e => e.OyKullananSecmenSayisi).HasColumnName("oy_KULLANAN_SECMEN_SAYISI");
            entity.Property(e => e.Parti10AldigiOy).HasColumnName("parti10_ALDIGI_OY");
            entity.Property(e => e.Parti11AldigiOy).HasColumnName("parti11_ALDIGI_OY");
            entity.Property(e => e.Parti12AldigiOy).HasColumnName("parti12_ALDIGI_OY");
            entity.Property(e => e.Parti13AldigiOy).HasColumnName("parti13_ALDIGI_OY");
            entity.Property(e => e.Parti14AldigiOy).HasColumnName("parti14_ALDIGI_OY");
            entity.Property(e => e.Parti15AldigiOy).HasColumnName("parti15_ALDIGI_OY");
            entity.Property(e => e.Parti16AldigiOy).HasColumnName("parti16_ALDIGI_OY");
            entity.Property(e => e.Parti17AldigiOy).HasColumnName("parti17_ALDIGI_OY");
            entity.Property(e => e.Parti18AldigiOy).HasColumnName("parti18_ALDIGI_OY");
            entity.Property(e => e.Parti19AldigiOy).HasColumnName("parti19_ALDIGI_OY");
            entity.Property(e => e.Parti1AldigiOy).HasColumnName("parti1_ALDIGI_OY");
            entity.Property(e => e.Parti20AldigiOy).HasColumnName("parti20_ALDIGI_OY");
            entity.Property(e => e.Parti21AldigiOy).HasColumnName("parti21_ALDIGI_OY");
            entity.Property(e => e.Parti22AldigiOy).HasColumnName("parti22_ALDIGI_OY");
            entity.Property(e => e.Parti23AldigiOy).HasColumnName("parti23_ALDIGI_OY");
            entity.Property(e => e.Parti24AldigiOy).HasColumnName("parti24_ALDIGI_OY");
            entity.Property(e => e.Parti25AldigiOy).HasColumnName("parti25_ALDIGI_OY");
            entity.Property(e => e.Parti26AldigiOy).HasColumnName("parti26_ALDIGI_OY");
            entity.Property(e => e.Parti27AldigiOy).HasColumnName("parti27_ALDIGI_OY");
            entity.Property(e => e.Parti28AldigiOy).HasColumnName("parti28_ALDIGI_OY");
            entity.Property(e => e.Parti29AldigiOy).HasColumnName("parti29_ALDIGI_OY");
            entity.Property(e => e.Parti2AldigiOy).HasColumnName("parti2_ALDIGI_OY");
            entity.Property(e => e.Parti30AldigiOy).HasColumnName("parti30_ALDIGI_OY");
            entity.Property(e => e.Parti3AldigiOy).HasColumnName("parti3_ALDIGI_OY");
            entity.Property(e => e.Parti4AldigiOy).HasColumnName("parti4_ALDIGI_OY");
            entity.Property(e => e.Parti5AldigiOy).HasColumnName("parti5_ALDIGI_OY");
            entity.Property(e => e.Parti6AldigiOy).HasColumnName("parti6_ALDIGI_OY");
            entity.Property(e => e.Parti7AldigiOy).HasColumnName("parti7_ALDIGI_OY");
            entity.Property(e => e.Parti8AldigiOy).HasColumnName("parti8_ALDIGI_OY");
            entity.Property(e => e.Parti9AldigiOy).HasColumnName("parti9_ALDIGI_OY");
            entity.Property(e => e.SandikDokumCetveli).HasColumnName("sandik_DOKUM_CETVELI");
            entity.Property(e => e.SandikId).HasColumnName("sandik_ID");
            entity.Property(e => e.SandikNo).HasColumnName("sandik_NO");
            entity.Property(e => e.SandikRumuz)
                .HasMaxLength(50)
                .HasColumnName("sandik_RUMUZ");
            entity.Property(e => e.SandikSonucId).HasColumnName("sandik_SONUC_ID");
            entity.Property(e => e.SandikSonucTutanagi).HasColumnName("sandik_SONUC_TUTANAGI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_cevresi_id");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTuru).HasColumnName("secim_TURU");
            entity.Property(e => e.SecmenSayisi).HasColumnName("secmen_SAYISI");
            entity.Property(e => e.SonIslemTarihi)
                .HasMaxLength(50)
                .HasColumnName("son_ISLEM_TARIHI");
            entity.Property(e => e.UlkeId).HasColumnName("ulke_ID");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
        });

        modelBuilder.Entity<SandikSonucTutanaklari>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sandik_sonuc_tutanaklari", "genel");

            entity.Property(e => e.BirlestirmeId).HasColumnName("birlestirme_ID");
            entity.Property(e => e.DetailId).HasColumnName("detail_ID");
            entity.Property(e => e.FileType).HasColumnName("file_TYPE");
            entity.Property(e => e.FinishTime)
                .HasMaxLength(50)
                .HasColumnName("finish_TIME");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PageNo).HasColumnName("page_NO");
            entity.Property(e => e.PageTotal).HasColumnName("page_TOTAL");
            entity.Property(e => e.SandikId).HasColumnName("sandik_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTuru).HasColumnName("secim_TURU");
            entity.Property(e => e.UlkeId).HasColumnName("ulke_ID");
        });

        modelBuilder.Entity<Secim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_pk");

            entity.ToTable("secim", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdaylikKesinlesmeTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("adaylik_kesinlesme_tarihi");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.Arasecim).HasColumnName("arasecim");
            entity.Property(e => e.BaslangicTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("baslangic_tarihi");
            entity.Property(e => e.ResmiGazeteTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("resmi_gazete_tarihi");
            entity.Property(e => e.SecimTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("secim_tarihi");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.SecmenSonHareketTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("secmen_son_hareket_tarihi");
            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.Yenilemesecimi).HasColumnName("yenilemesecimi");
            entity.Property(e => e.YenilenenSecimId).HasColumnName("yenilenen_secim_id");
            entity.Property(e => e.YskSecimId).HasColumnName("ysk_secim_id");
            entity.Property(e => e.YskYenilenenSecimId).HasColumnName("ysk_yenilenen_secim_id");
            entity.Property(e => e.YurtdisiSayi).HasColumnName("yurtdisi_sayi");
        });

        modelBuilder.Entity<SecimDetay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_detay_pk");

            entity.ToTable("secim_detay", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcilanSandik).HasColumnName("acilan_sandik");
            entity.Property(e => e.AcilanSecmen).HasColumnName("acilan_secmen");
            entity.Property(e => e.AdaySayisi).HasColumnName("aday_sayisi");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.BagimsizOy)
                .HasDefaultValueSql("0")
                .HasColumnName("bagimsiz_oy");
            entity.Property(e => e.BirlestirmeSayi).HasColumnName("birlestirme_sayi");
            entity.Property(e => e.GecerliOy).HasColumnName("gecerli_oy");
            entity.Property(e => e.GecersizOy).HasColumnName("gecersiz_oy");
            entity.Property(e => e.GumrukSayi).HasColumnName("gumruk_sayi");
            entity.Property(e => e.ItirazliGecerliOy).HasColumnName("itirazli_gecerli_oy");
            entity.Property(e => e.ItirazsizGecerliOy).HasColumnName("itirazsiz_gecerli_oy");
            entity.Property(e => e.OyKullanan).HasColumnName("oy_kullanan");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.SecmenSayisi).HasColumnName("secmen_sayisi");
            entity.Property(e => e.ToplamSandik).HasColumnName("toplam_sandik");
            entity.Property(e => e.TutanakSayi).HasColumnName("tutanak_sayi");
            entity.Property(e => e.YskSecimDetayId).HasColumnName("ysk_secim_detay_id");
            entity.Property(e => e.YskSecimId).HasColumnName("ysk_secim_id");
            entity.Property(e => e.YurtdisiSayi).HasColumnName("yurtdisi_sayi");

            entity.HasOne(d => d.Secim).WithMany(p => p.SecimDetays)
                .HasForeignKey(d => d.SecimId)
                .HasConstraintName("secim_detay_secimid_fk");

            entity.HasOne(d => d.SecimTuru).WithMany(p => p.SecimDetays)
                .HasForeignKey(d => d.SecimTuruId)
                .HasConstraintName("secim_detay_secimturu_fk");
        });

        modelBuilder.Entity<SecimDetayListesi>(entity =>
        {
            entity.HasKey(e => e.SecimDetayId).HasName("secim_detay_listesi_pk");

            entity.ToTable("secim_detay_listesi", "genel");

            entity.Property(e => e.SecimDetayId)
                .ValueGeneratedNever()
                .HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.AskiBaslangicTarihi).HasColumnName("aski_BASLANGIC_TARIHI");
            entity.Property(e => e.AskiBitisTarihi)
                .HasMaxLength(50)
                .HasColumnName("aski_BITIS_TARIHI");
            entity.Property(e => e.BirlestirmeSayi).HasColumnName("birlestirme_SAYI");
            entity.Property(e => e.GumrukSayi).HasColumnName("gumruk_SAYI");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTuruAdi)
                .HasMaxLength(50)
                .HasColumnName("secim_TURU_ADI");
            entity.Property(e => e.SecimTuruKodu).HasColumnName("secim_TURU_KODU");
            entity.Property(e => e.TutanakSayi).HasColumnName("tutanak_SAYI");
            entity.Property(e => e.YurtdisiSayi).HasColumnName("yurtdisi_SAYI");
        });

        modelBuilder.Entity<SecimIlListesi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_il_listesi_pk");

            entity.ToTable("secim_il_listesi", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
        });

        modelBuilder.Entity<SecimIlceListesi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_ilce_listesi_pk");

            entity.ToTable("secim_ilce_listesi", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.IlId).HasColumnName("il_id");

            entity.HasOne(d => d.Il).WithMany(p => p.SecimIlceListesis)
                .HasForeignKey(d => d.IlId)
                .HasConstraintName("secim_ilce_listesi_ilid_fk");
        });

        modelBuilder.Entity<SecimIlceler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_ilceler_pk");

            entity.ToTable("secim_ilceler", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.YskSecimCevresiId).HasColumnName("ysk_secim_cevresi_id");
            entity.Property(e => e.YskSecimId).HasColumnName("ysk_secim_id");

            entity.HasOne(d => d.Il).WithMany(p => p.SecimIlcelers)
                .HasForeignKey(d => d.IlId)
                .HasConstraintName("secim_ilceler_fk");

            entity.HasOne(d => d.Ilce).WithMany(p => p.SecimIlcelers)
                .HasForeignKey(d => d.IlceId)
                .HasConstraintName("secim_ilceler_ilceid_fk");

            entity.HasOne(d => d.SecimDetay).WithMany(p => p.SecimIlcelers)
                .HasForeignKey(d => d.SecimDetayId)
                .HasConstraintName("secim_ilceler_secidetayid_fk");
        });

        modelBuilder.Entity<SecimIller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_iller_pk");

            entity.ToTable("secim_iller", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdaySayisi).HasColumnName("aday_sayisi");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.YskSecimCevresiId).HasColumnName("ysk_secim_cevresi_id");
            entity.Property(e => e.YskSecimDetayId).HasColumnName("ysk_secim_detay_id");
            entity.Property(e => e.YskSecimId).HasColumnName("ysk_secim_id");

            entity.HasOne(d => d.Il).WithMany(p => p.SecimIllers)
                .HasForeignKey(d => d.IlId)
                .HasConstraintName("secim_iller_ilid_fk");

            entity.HasOne(d => d.SecimDetay).WithMany(p => p.SecimIllers)
                .HasForeignKey(d => d.SecimDetayId)
                .HasConstraintName("secim_iller_secimdetayid_fk");
        });

        modelBuilder.Entity<SecimIttifaklar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_ittifaklar_pk");

            entity.ToTable("secim_ittifaklar", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SonucBaslik)
                .HasMaxLength(75)
                .HasColumnName("sonuc_baslik");
            entity.Property(e => e.YskPartiId).HasColumnName("ysk_parti_id");
            entity.Property(e => e.YurtDisiTolamOy).HasColumnName("yurt_disi_tolam_oy");
            entity.Property(e => e.YurtIciTolamOy).HasColumnName("yurt_ici_tolam_oy");
        });

        modelBuilder.Entity<SecimMahalleListesi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_mahalle_listesi_pk");

            entity.ToTable("secim_mahalle_listesi", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.YskMuhtarlikId).HasColumnName("ysk_muhtarlik_id");
        });

        modelBuilder.Entity<SecimMahalleSecimBilgisi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("election_mahalle_secim_bilgisi_pk");

            entity.ToTable("secim_mahalle_secim_bilgisi", "election");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('election.mahalle_secim_bilgisi_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.MahalleId).HasColumnName("mahalle_id");
            entity.Property(e => e.MaxSandikNo).HasColumnName("max_sandik_no");
            entity.Property(e => e.MinSandikNo).HasColumnName("min_sandik_no");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
        });

        modelBuilder.Entity<SecimMuhtarlikListe>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("secim_muhtarlik_liste_pk");

            entity.ToTable("secim_muhtarlik_liste", "genel");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.Alindi).HasColumnName("alindi");
            entity.Property(e => e.BeldeId).HasColumnName("belde_ID");
            entity.Property(e => e.BirimId).HasColumnName("birim_ID");
            entity.Property(e => e.CezaeviId).HasColumnName("cezaevi_ID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(50)
                .HasColumnName("il_ADI");
            entity.Property(e => e.IlId).HasColumnName("il_ID");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(50)
                .HasColumnName("ilce_ADI");
            entity.Property(e => e.IlceId).HasColumnName("ilce_ID");
            entity.Property(e => e.Isnew)
                .HasDefaultValueSql("true")
                .HasColumnName("isnew");
            entity.Property(e => e.MaxSandikNo).HasColumnName("max_SANDIK_NO");
            entity.Property(e => e.MinSandikNo).HasColumnName("min_SANDIK_NO");
            entity.Property(e => e.MuhtarlikAdi)
                .HasMaxLength(500)
                .HasColumnName("muhtarlik_ADI");
            entity.Property(e => e.MuhtarlikId).HasColumnName("muhtarlik_ID");
            entity.Property(e => e.Mukerrer).HasColumnName("mukerrer");
            entity.Property(e => e.SecilecekAdaySayisi).HasColumnName("secilecek_ADAY_SAYISI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_CEVRESI_ID");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.YerlesimYeriTuru).HasColumnName("yerlesim_YERI_TURU");
        });

        modelBuilder.Entity<SecimMuhtarlikOzet>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("secim_muhtarlik_ozet", "election");

            entity.HasIndex(e => new { e.SecimDetayId, e.SecimIllerId }, "secim_muhtarlik_ozet_secim_detay_id_idx");

            entity.Property(e => e.Bagimsiz).HasColumnName("bagimsiz");
            entity.Property(e => e.Gecerli).HasColumnName("gecerli");
            entity.Property(e => e.Gecersiz).HasColumnName("gecersiz");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.IlceId).HasColumnName("ilce_ID");
            entity.Property(e => e.MuhtarlikId).HasColumnName("muhtarlik_id");
            entity.Property(e => e.OyKullanan).HasColumnName("oy_kullanan");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_cevresi_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimIlcelerId).HasColumnName("secim_ilceler_id");
            entity.Property(e => e.SecimIllerId).HasColumnName("secim_iller_id");
            entity.Property(e => e.Secmen).HasColumnName("secmen");
            entity.Property(e => e.YskMuhtarlikId).HasColumnName("ysk_muhtarlik_id");
        });

        modelBuilder.Entity<SecimMuhtarlikSandiklar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_muhtarlik_sandiklar_pk");

            entity.ToTable("secim_muhtarlik_sandiklar", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isok)
                .HasDefaultValueSql("false")
                .HasColumnName("isok");
            entity.Property(e => e.SandikNo).HasColumnName("sandik_no");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimMuhtarlikId).HasColumnName("secim_muhtarlik_id");
            entity.Property(e => e.YskIlceId).HasColumnName("ysk_ilce_id");
            entity.Property(e => e.YskMuhtarlikId).HasColumnName("ysk_muhtarlik_id");
        });

        modelBuilder.Entity<SecimMuhtarliklar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_muhtarliklar_pk");

            entity.ToTable("secim_muhtarliklar", "election");

            entity.HasIndex(e => e.YskMuhtarlikId, "secim_muhtarliklar_ysk_muhtarlik_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.MaxSandikNo).HasColumnName("max_sandik_no");
            entity.Property(e => e.MinSandikNo).HasColumnName("min_sandik_no");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.YskIlceId).HasColumnName("ysk_ilce_id");
            entity.Property(e => e.YskMuhtarlikId).HasColumnName("ysk_muhtarlik_id");

            entity.HasOne(d => d.Ilce).WithMany(p => p.SecimMuhtarliklars)
                .HasForeignKey(d => d.IlceId)
                .HasConstraintName("secim_muhtarliklar_fk");
        });

        modelBuilder.Entity<SecimPartiler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_partiler_pk");

            entity.ToTable("secim_partiler", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartiId).HasColumnName("parti_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SonucBaslik)
                .HasMaxLength(75)
                .HasColumnName("sonuc_baslik");
            entity.Property(e => e.YskPartiId).HasColumnName("ysk_parti_id");
            entity.Property(e => e.YurtDisiToplamOy).HasColumnName("yurt_disi_toplam_oy");
            entity.Property(e => e.YurtIciToplamOy).HasColumnName("yurt_ici_toplam_oy");

            entity.HasOne(d => d.Parti).WithMany(p => p.SecimPartilers)
                .HasForeignKey(d => d.PartiId)
                .HasConstraintName("secim_partiler_partiid_fk");

            entity.HasOne(d => d.SecimDetay).WithMany(p => p.SecimPartilers)
                .HasForeignKey(d => d.SecimDetayId)
                .HasConstraintName("secim_partiler_secimdetayid_fk");
        });

        modelBuilder.Entity<SecimSandikSonuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_sandik_sonuc_pk");

            entity.ToTable("secim_sandik_sonuc", "election");

            entity.HasIndex(e => e.PartiId, "secim_sandik_sonuc_parti_id_idx");

            entity.HasIndex(e => e.YskMuhtarlikId, "secim_sandik_sonuc_ysk_muhtarlik_id_idx");

            entity.HasIndex(e => e.SecimDetayId, "sss_secimdetay_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MuhtarlikId).HasColumnName("muhtarlik_id");
            entity.Property(e => e.OySayisi).HasColumnName("oy_sayisi");
            entity.Property(e => e.PartiId).HasColumnName("parti_id");
            entity.Property(e => e.SandikNo).HasColumnName("sandik_no");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.YskMuhtarlikId).HasColumnName("ysk_muhtarlik_id");

            entity.HasOne(d => d.Muhtarlik).WithMany(p => p.SecimSandikSonucs)
                .HasForeignKey(d => d.MuhtarlikId)
                .HasConstraintName("secim_sandik_sonuc_fk");

            entity.HasOne(d => d.Parti).WithMany(p => p.SecimSandikSonucs)
                .HasForeignKey(d => d.PartiId)
                .HasConstraintName("secim_sandik_sonuc_partiid_fk");

            entity.HasOne(d => d.SecimDetay).WithMany(p => p.SecimSandikSonucs)
                .HasForeignKey(d => d.SecimDetayId)
                .HasConstraintName("secim_sandik_sonuc_secimdetayid_fk");
        });

        modelBuilder.Entity<SecimSandikSonucList>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("secim_sandik_sonuc_list_pk");

            entity.ToTable("secim_sandik_sonuc_list", "genel");

            entity.HasIndex(e => e.SecimId, "secim_sandik_sonuc_list_secim_id_idx");

            entity.HasIndex(e => e.MuhtarlikId, "secim_sandiksonucsandikindx");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.Bagimsiz10AldigiOy).HasColumnName("bagimsiz10_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz11AldigiOy).HasColumnName("bagimsiz11_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz12AldigiOy).HasColumnName("bagimsiz12_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz13AldigiOy).HasColumnName("bagimsiz13_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz14AldigiOy).HasColumnName("bagimsiz14_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz15AldigiOy).HasColumnName("bagimsiz15_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz16AldigiOy).HasColumnName("bagimsiz16_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz17AldigiOy).HasColumnName("bagimsiz17_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz18AldigiOy).HasColumnName("bagimsiz18_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz19AldigiOy).HasColumnName("bagimsiz19_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz1AldigiOy).HasColumnName("bagimsiz1_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz20AldigiOy).HasColumnName("bagimsiz20_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz21AldigiOy).HasColumnName("bagimsiz21_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz22AldigiOy).HasColumnName("bagimsiz22_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz23AldigiOy).HasColumnName("bagimsiz23_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz24AldigiOy).HasColumnName("bagimsiz24_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz25AldigiOy).HasColumnName("bagimsiz25_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz26AldigiOy).HasColumnName("bagimsiz26_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz27AldigiOy).HasColumnName("bagimsiz27_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz28AldigiOy).HasColumnName("bagimsiz28_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz29AldigiOy).HasColumnName("bagimsiz29_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz2AldigiOy).HasColumnName("bagimsiz2_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz30AldigiOy).HasColumnName("bagimsiz30_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz31AldigiOy).HasColumnName("bagimsiz31_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz32AldigiOy).HasColumnName("bagimsiz32_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz33AldigiOy).HasColumnName("bagimsiz33_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz34AldigiOy).HasColumnName("bagimsiz34_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz35AldigiOy).HasColumnName("bagimsiz35_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz36AldigiOy).HasColumnName("bagimsiz36_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz37AldigiOy).HasColumnName("bagimsiz37_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz38AldigiOy).HasColumnName("bagimsiz38_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz39AldigiOy).HasColumnName("bagimsiz39_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz3AldigiOy).HasColumnName("bagimsiz3_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz40AldigiOy).HasColumnName("bagimsiz40_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz41AldigiOy).HasColumnName("bagimsiz41_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz42AldigiOy).HasColumnName("bagimsiz42_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz43AldigiOy).HasColumnName("bagimsiz43_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz44AldigiOy).HasColumnName("bagimsiz44_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz45AldigiOy).HasColumnName("bagimsiz45_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz46AldigiOy).HasColumnName("bagimsiz46_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz47AldigiOy).HasColumnName("bagimsiz47_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz48AldigiOy).HasColumnName("bagimsiz48_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz49AldigiOy).HasColumnName("bagimsiz49_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz4AldigiOy).HasColumnName("bagimsiz4_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz50AldigiOy).HasColumnName("bagimsiz50_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz5AldigiOy).HasColumnName("bagimsiz5_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz6AldigiOy).HasColumnName("bagimsiz6_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz7AldigiOy).HasColumnName("bagimsiz7_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz8AldigiOy).HasColumnName("bagimsiz8_ALDIGI_OY");
            entity.Property(e => e.Bagimsiz9AldigiOy).HasColumnName("bagimsiz9_ALDIGI_OY");
            entity.Property(e => e.BagimsizToplamOy).HasColumnName("bagimsiz_TOPLAM_OY");
            entity.Property(e => e.BeldeId).HasColumnName("belde_ID");
            entity.Property(e => e.BirimId).HasColumnName("birim_ID");
            entity.Property(e => e.BirlestirmeTutanagiDisTemsilcilik).HasColumnName("birlestirme_TUTANAGI_DIS_TEMSILCILIK");
            entity.Property(e => e.BirlestirmeTutanagiGumrukIlce).HasColumnName("birlestirme_TUTANAGI_GUMRUK_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiGumrukKurul).HasColumnName("birlestirme_TUTANAGI_GUMRUK_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiGumrukRumuz).HasColumnName("birlestirme_TUTANAGI_GUMRUK_RUMUZ");
            entity.Property(e => e.BirlestirmeTutanagiGumrukler).HasColumnName("birlestirme_TUTANAGI_GUMRUKLER");
            entity.Property(e => e.BirlestirmeTutanagiIl).HasColumnName("birlestirme_TUTANAGI_IL");
            entity.Property(e => e.BirlestirmeTutanagiIlce).HasColumnName("birlestirme_TUTANAGI_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiKurul).HasColumnName("birlestirme_TUTANAGI_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiTumdunya).HasColumnName("birlestirme_TUTANAGI_TUMDUNYA");
            entity.Property(e => e.BirlestirmeTutanagiUlke).HasColumnName("birlestirme_TUTANAGI_ULKE");
            entity.Property(e => e.BirlestirmeTutanagiUlkeler).HasColumnName("birlestirme_TUTANAGI_ULKELER");
            entity.Property(e => e.CezaeviId).HasColumnName("cezaevi_ID");
            entity.Property(e => e.DegisiklikNedeni).HasColumnName("degisiklik_NEDENI");
            entity.Property(e => e.DenetimTutanagi).HasColumnName("denetim_TUTANAGI");
            entity.Property(e => e.DisTemsilcilikId).HasColumnName("dis_TEMSILCILIK_ID");
            entity.Property(e => e.GecerliOyToplami).HasColumnName("gecerli_OY_TOPLAMI");
            entity.Property(e => e.GecersizOyToplami).HasColumnName("gecersiz_OY_TOPLAMI");
            entity.Property(e => e.GumrukId).HasColumnName("gumruk_ID");
            entity.Property(e => e.GumrukSandikTarihi).HasColumnName("gumruk_SANDIK_TARIHI");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(250)
                .HasColumnName("il_ADI");
            entity.Property(e => e.IlId).HasColumnName("il_ID");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(250)
                .HasColumnName("ilce_ADI");
            entity.Property(e => e.IlceId).HasColumnName("ilce_ID");
            entity.Property(e => e.ItirazliGecerliOySayisi).HasColumnName("itirazli_GECERLI_OY_SAYISI");
            entity.Property(e => e.ItirazsizGecerliOySayisi).HasColumnName("itirazsiz_GECERLI_OY_SAYISI");
            entity.Property(e => e.Ittifak1AldigiOy).HasColumnName("ittifak1_ALDIGI_OY");
            entity.Property(e => e.Ittifak2AldigiOy).HasColumnName("ittifak2_ALDIGI_OY");
            entity.Property(e => e.Ittifak3AldigiOy).HasColumnName("ittifak3_ALDIGI_OY");
            entity.Property(e => e.Ittifak4AldigiOy).HasColumnName("ittifak4_ALDIGI_OY");
            entity.Property(e => e.Ittifak5AldigiOy).HasColumnName("ittifak5_ALDIGI_OY");
            entity.Property(e => e.MuhtarlikAdi)
                .HasMaxLength(250)
                .HasColumnName("muhtarlik_ADI");
            entity.Property(e => e.MuhtarlikId).HasColumnName("muhtarlik_ID");
            entity.Property(e => e.OyKullananSecmenSayisi).HasColumnName("oy_KULLANAN_SECMEN_SAYISI");
            entity.Property(e => e.Parti10AldigiOy).HasColumnName("parti10_ALDIGI_OY");
            entity.Property(e => e.Parti11AldigiOy).HasColumnName("parti11_ALDIGI_OY");
            entity.Property(e => e.Parti12AldigiOy).HasColumnName("parti12_ALDIGI_OY");
            entity.Property(e => e.Parti13AldigiOy).HasColumnName("parti13_ALDIGI_OY");
            entity.Property(e => e.Parti14AldigiOy).HasColumnName("parti14_ALDIGI_OY");
            entity.Property(e => e.Parti15AldigiOy).HasColumnName("parti15_ALDIGI_OY");
            entity.Property(e => e.Parti16AldigiOy).HasColumnName("parti16_ALDIGI_OY");
            entity.Property(e => e.Parti17AldigiOy).HasColumnName("parti17_ALDIGI_OY");
            entity.Property(e => e.Parti18AldigiOy).HasColumnName("parti18_ALDIGI_OY");
            entity.Property(e => e.Parti19AldigiOy).HasColumnName("parti19_ALDIGI_OY");
            entity.Property(e => e.Parti1AldigiOy).HasColumnName("parti1_ALDIGI_OY");
            entity.Property(e => e.Parti20AldigiOy).HasColumnName("parti20_ALDIGI_OY");
            entity.Property(e => e.Parti21AldigiOy).HasColumnName("parti21_ALDIGI_OY");
            entity.Property(e => e.Parti22AldigiOy).HasColumnName("parti22_ALDIGI_OY");
            entity.Property(e => e.Parti23AldigiOy).HasColumnName("parti23_ALDIGI_OY");
            entity.Property(e => e.Parti24AldigiOy).HasColumnName("parti24_ALDIGI_OY");
            entity.Property(e => e.Parti25AldigiOy).HasColumnName("parti25_ALDIGI_OY");
            entity.Property(e => e.Parti26AldigiOy).HasColumnName("parti26_ALDIGI_OY");
            entity.Property(e => e.Parti27AldigiOy).HasColumnName("parti27_ALDIGI_OY");
            entity.Property(e => e.Parti28AldigiOy).HasColumnName("parti28_ALDIGI_OY");
            entity.Property(e => e.Parti29AldigiOy).HasColumnName("parti29_ALDIGI_OY");
            entity.Property(e => e.Parti2AldigiOy).HasColumnName("parti2_ALDIGI_OY");
            entity.Property(e => e.Parti30AldigiOy).HasColumnName("parti30_ALDIGI_OY");
            entity.Property(e => e.Parti3AldigiOy).HasColumnName("parti3_ALDIGI_OY");
            entity.Property(e => e.Parti4AldigiOy).HasColumnName("parti4_ALDIGI_OY");
            entity.Property(e => e.Parti5AldigiOy).HasColumnName("parti5_ALDIGI_OY");
            entity.Property(e => e.Parti6AldigiOy).HasColumnName("parti6_ALDIGI_OY");
            entity.Property(e => e.Parti7AldigiOy).HasColumnName("parti7_ALDIGI_OY");
            entity.Property(e => e.Parti8AldigiOy).HasColumnName("parti8_ALDIGI_OY");
            entity.Property(e => e.Parti9AldigiOy).HasColumnName("parti9_ALDIGI_OY");
            entity.Property(e => e.SandikDokumCetveli).HasColumnName("sandik_DOKUM_CETVELI");
            entity.Property(e => e.SandikId).HasColumnName("sandik_ID");
            entity.Property(e => e.SandikNo).HasColumnName("sandik_NO");
            entity.Property(e => e.SandikRumuz)
                .HasMaxLength(250)
                .HasColumnName("sandik_RUMUZ");
            entity.Property(e => e.SandikSonucId).HasColumnName("sandik_SONUC_ID");
            entity.Property(e => e.SandikSonucTutanagi).HasColumnName("sandik_SONUC_TUTANAGI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_cevresi_id");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTuru).HasColumnName("secim_TURU");
            entity.Property(e => e.SecmenSayisi).HasColumnName("secmen_SAYISI");
            entity.Property(e => e.SonIslemTarihi).HasColumnName("son_ISLEM_TARIHI");
            entity.Property(e => e.UlkeId).HasColumnName("ulke_ID");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
        });

        modelBuilder.Entity<SecimSonucBasliklari>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("secim_sonuc_basliklari", "genel");

            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.ColumnName)
                .HasMaxLength(50)
                .HasColumnName("column_NAME");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_cevresi_id");
            entity.Property(e => e.SecimId).HasColumnName("secim_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.SiraNo).HasColumnName("sira_NO");
        });

        modelBuilder.Entity<SecimSonucListesi>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("secim_sonuc_listesi_pk");

            entity.ToTable("secim_sonuc_listesi", "genel");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.AcilanSandikSayisi).HasColumnName("acilan_SANDIK_SAYISI");
            entity.Property(e => e.AcilanSecmenSayisi).HasColumnName("acilan_SECMEN_SAYISI");
            entity.Property(e => e.BirlestirmeTutanagiDisTemsilcilik).HasColumnName("birlestirme_TUTANAGI_DIS_TEMSILCILIK");
            entity.Property(e => e.BirlestirmeTutanagiGumrukIlce).HasColumnName("birlestirme_TUTANAGI_GUMRUK_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiGumrukKurul).HasColumnName("birlestirme_TUTANAGI_GUMRUK_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiGumrukRumuz).HasColumnName("birlestirme_TUTANAGI_GUMRUK_RUMUZ");
            entity.Property(e => e.BirlestirmeTutanagiGumrukler).HasColumnName("birlestirme_TUTANAGI_GUMRUKLER");
            entity.Property(e => e.BirlestirmeTutanagiIl).HasColumnName("birlestirme_TUTANAGI_IL");
            entity.Property(e => e.BirlestirmeTutanagiIlce).HasColumnName("birlestirme_TUTANAGI_ILCE");
            entity.Property(e => e.BirlestirmeTutanagiKurul).HasColumnName("birlestirme_TUTANAGI_KURUL");
            entity.Property(e => e.BirlestirmeTutanagiTumdunya).HasColumnName("birlestirme_TUTANAGI_TUMDUNYA");
            entity.Property(e => e.BirlestirmeTutanagiUlke).HasColumnName("birlestirme_TUTANAGI_ULKE");
            entity.Property(e => e.BirlestirmeTutanagiUlkeler).HasColumnName("birlestirme_TUTANAGI_ULKELER");
            entity.Property(e => e.GecerliOyToplami).HasColumnName("gecerli_OY_TOPLAMI");
            entity.Property(e => e.GecersizOyToplami).HasColumnName("gecersiz_OY_TOPLAMI");
            entity.Property(e => e.ItirazliGecerliOySayisi).HasColumnName("itirazli_GECERLI_OY_SAYISI");
            entity.Property(e => e.ItirazsizGecerliOySayisi).HasColumnName("itirazsiz_GECERLI_OY_SAYISI");
            entity.Property(e => e.OyKullananSecmenSayisi).HasColumnName("oy_KULLANAN_SECMEN_SAYISI");
            entity.Property(e => e.SecilecekAdaySayisi).HasColumnName("secilecek_ADAY_SAYISI");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimTuruId).HasColumnName("secim_turu_id");
            entity.Property(e => e.SecmenSayisi).HasColumnName("secmen_SAYISI");
            entity.Property(e => e.ToplamSandikSayisi).HasColumnName("toplam_SANDIK_SAYISI");
        });

        modelBuilder.Entity<SecimSonuclar>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("secim_sonuclar", "election");

            entity.HasIndex(e => new { e.SecimDetayId, e.YskSecimCevresiId, e.SecimIllerId }, "secim_sonuclar_secim_detay_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.OySayisi).HasColumnName("oy_sayisi");
            entity.Property(e => e.PartiId).HasColumnName("parti_id");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimIlcelerId).HasColumnName("secim_ilceler_id");
            entity.Property(e => e.SecimIllerId).HasColumnName("secim_iller_id");
            entity.Property(e => e.YskSecimCevresiId).HasColumnName("ysk_secim_cevresi_id");
        });

        modelBuilder.Entity<SecimTurleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("secim_turleri_pk");

            entity.ToTable("secim_turleri", "election");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .HasColumnName("adi");
        });

        modelBuilder.Entity<Secimler>(entity =>
        {
            entity.HasKey(e => e.TekilId).HasName("secimler_pk");

            entity.ToTable("secimler", "genel");

            entity.Property(e => e.TekilId).HasColumnName("tekil_id");
            entity.Property(e => e.AdaylikKesinlesmeTarihi).HasColumnName("adaylik_KESINLESME_TARIHI");
            entity.Property(e => e.AraSecim).HasColumnName("ara_SECIM");
            entity.Property(e => e.BirimId).HasColumnName("birim_ID");
            entity.Property(e => e.ResmiGazeteTarihi).HasColumnName("resmi_GAZETE_TARIHI");
            entity.Property(e => e.SecimAdi)
                .HasMaxLength(250)
                .HasColumnName("secim_ADI");
            entity.Property(e => e.SecimBaslangicTarihi).HasColumnName("secim_BASLANGIC_TARIHI");
            entity.Property(e => e.SecimDetaySayi).HasColumnName("secim_DETAY_SAYI");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTarihi).HasColumnName("secim_TARIHI");
            entity.Property(e => e.SecimTipi)
                .HasMaxLength(50)
                .HasColumnName("secim_TIPI");
            entity.Property(e => e.SecmenDegisiklikListeTarihi).HasColumnName("secmen_DEGISIKLIK_LISTE_TARIHI");
            entity.Property(e => e.SecmenSonHareketTarihi).HasColumnName("secmen_SON_HAREKET_TARIHI");
            entity.Property(e => e.YenilemeSecimi).HasColumnName("yenileme_SECIMI");
            entity.Property(e => e.YenilemeVeriOlusturmaTarihi).HasColumnName("yenileme_VERI_OLUSTURMA_TARIHI");
            entity.Property(e => e.YenilemeVerisiOlusturanPers)
                .HasMaxLength(1)
                .HasColumnName("yenileme_VERISI_OLUSTURAN_PERS");
            entity.Property(e => e.YenilenenSecimId).HasColumnName("yenilenen_SECIM_ID");
            entity.Property(e => e.YurtdisiSayi).HasColumnName("yurtdisi_SAYI");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sectors_pk");

            entity.ToTable("sectors", "combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.Weightiness).HasColumnName("weightiness");
        });

        modelBuilder.Entity<SessionCache>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DistCache_pkey");

            entity.ToTable("session_cache", "session");
        });

        modelBuilder.Entity<SocialMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("socialmedia_pk");

            entity.ToTable("social_media", "genel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('genel.socialmedia_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.SocialAddress)
                .HasColumnType("character varying")
                .HasColumnName("social_address");
            entity.Property(e => e.SocialTypeId).HasColumnName("social_type_id");

            entity.HasOne(d => d.SocialType).WithMany(p => p.SocialMedia)
                .HasForeignKey(d => d.SocialTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("social_media_fk");
        });

        modelBuilder.Entity<SocialType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("socials_pk");

            entity.ToTable("social_types", "combos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('combos.socials_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTr)
                .HasMaxLength(100)
                .HasColumnName("name_tr");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
            entity.Property(e => e.UrlPattern)
                .HasMaxLength(250)
                .HasColumnName("url_pattern");
        });

        modelBuilder.Entity<Sonuclar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("sonuclar", "election");

            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.MaxSandikNo).HasColumnName("max_sandik_no");
            entity.Property(e => e.MinSandikNo).HasColumnName("min_sandik_no");
            entity.Property(e => e.OySayisi).HasColumnName("oy_sayisi");
            entity.Property(e => e.PartiId).HasColumnName("parti_id");
            entity.Property(e => e.SandikNo).HasColumnName("sandik_no");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_detay_id");
            entity.Property(e => e.SecimIlceListesiId).HasColumnName("secim_ilce_listesi_id");
            entity.Property(e => e.SecimIlcelerId).HasColumnName("secim_ilceler_id");
            entity.Property(e => e.SecimIllerId).HasColumnName("secim_iller_id");
            entity.Property(e => e.SecimMuhtarliklarId).HasColumnName("secim_muhtarliklar_id");
            entity.Property(e => e.SecimSandikSonuId).HasColumnName("secim_sandik_sonu_id");
            entity.Property(e => e.YskSecimCevresiId).HasColumnName("ysk_secim_cevresi_id");
        });

        modelBuilder.Entity<VideoLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_library_pk");

            entity.ToTable("video_library", "genel");

            entity.HasIndex(e => e.Id, "video_library_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Head)
                .HasColumnType("character varying")
                .HasColumnName("head");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.Session).HasColumnName("session");
            entity.Property(e => e.VideoLink)
                .HasColumnType("character varying")
                .HasColumnName("video_link");
        });

        modelBuilder.Entity<VideoSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_session_pk");

            entity.ToTable("video_session", "combos");

            entity.HasIndex(e => e.Id, "video_session_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nameen)
                .HasColumnType("character varying")
                .HasColumnName("nameen");
            entity.Property(e => e.Nametr)
                .HasColumnType("character varying")
                .HasColumnName("nametr");
        });

        modelBuilder.Entity<YenilemeSecimList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("yenileme_secim_list", "genel");

            entity.Property(e => e.SecimAdi)
                .HasMaxLength(64)
                .HasColumnName("secim_ADI");
            entity.Property(e => e.SecimCevresiAdi)
                .HasMaxLength(50)
                .HasColumnName("secim_CEVRESI_ADI");
            entity.Property(e => e.SecimCevresiId).HasColumnName("secim_CEVRESI_ID");
            entity.Property(e => e.SecimDetayId).HasColumnName("secim_DETAY_ID");
            entity.Property(e => e.SecimId).HasColumnName("secim_ID");
            entity.Property(e => e.SecimTarihi).HasColumnName("secim_TARIHI");
            entity.Property(e => e.SecimTuruKodu).HasColumnName("secim_TURU_KODU");
            entity.Property(e => e.YerlesimYeriId).HasColumnName("yerlesim_YERI_ID");
            entity.Property(e => e.YerlesimYeriTuru).HasColumnName("yerlesim_YERI_TURU");
        });

        modelBuilder.Entity<YynDiller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_diller_pkey");

            entity.ToTable("yyn_diller", "yayinlar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasColumnType("character varying")
                .HasColumnName("adi");
        });

        modelBuilder.Entity<YynKategoriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kategoriler_pkey");

            entity.ToTable("yyn_kategoriler", "yayinlar");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('yayinlar.kategoriler_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(255)
                .HasColumnName("adi");
        });

        modelBuilder.Entity<YynKitapKategoriler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_kitap_kategoriler_pkey");

            entity.ToTable("yyn_kitap_kategoriler", "yayinlar");

            entity.HasIndex(e => e.KategoriId, "fki_kitap_kategori_kategoriid");

            entity.HasIndex(e => e.KitapId, "fki_kitap_kategori_kitapid_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KategoriId).HasColumnName("kategori_id");
            entity.Property(e => e.KitapId).HasColumnName("kitap_id");

            entity.HasOne(d => d.Kategori).WithMany(p => p.YynKitapKategorilers)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kitap_kategori_kategoriid");

            entity.HasOne(d => d.Kitap).WithMany(p => p.YynKitapKategorilers)
                .HasForeignKey(d => d.KitapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kitap_kategori_kitapid_fk");
        });

        modelBuilder.Entity<YynKitapYazar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_kitap_yazar_pk");

            entity.ToTable("yyn_kitap_yazar", "yayinlar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KitapId).HasColumnName("kitap_id");
            entity.Property(e => e.YazarId).HasColumnName("yazar_id");
        });

        modelBuilder.Entity<YynKitaplar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_kitaplar_pkey");

            entity.ToTable("yyn_kitaplar", "yayinlar");

            entity.HasIndex(e => e.DilId, "fki_kitap_dilid_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.Adi)
                .HasMaxLength(500)
                .HasColumnName("adi");
            entity.Property(e => e.DilId).HasColumnName("dil_id");
            entity.Property(e => e.EKitapUrl)
                .HasMaxLength(500)
                .HasColumnName("e_kitap_url");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.YayinEviId).HasColumnName("yayin_evi_id");
            entity.Property(e => e.Yili).HasColumnName("yili");

            entity.HasOne(d => d.Dil).WithMany(p => p.YynKitaplars)
                .HasForeignKey(d => d.DilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kitap_dilid_fk");
        });

        modelBuilder.Entity<YynKoseYazilari>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_kose_yazilari_pkey");

            entity.ToTable("yyn_kose_yazilari", "yayinlar");

            entity.HasIndex(e => e.DilId, "fki_kose_yazilari_dilid_fk");

            entity.HasIndex(e => e.TurId, "fki_kose_yazilari_turid_fk");

            entity.HasIndex(e => e.YazarId, "fki_kose_yazilari_yazarid_fk");

            entity.HasIndex(e => e.SirketId, "fki_s");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Baslik)
                .HasMaxLength(500)
                .HasColumnName("baslik");
            entity.Property(e => e.BaslikEn)
                .HasMaxLength(500)
                .HasColumnName("baslik_en");
            entity.Property(e => e.DilId)
                .HasDefaultValueSql("1")
                .HasColumnName("dil_id");
            entity.Property(e => e.Keywords)
                .HasColumnType("character varying")
                .HasColumnName("keywords");
            entity.Property(e => e.Metin).HasColumnName("metin");
            entity.Property(e => e.MetinEn).HasColumnName("metin_en");
            entity.Property(e => e.MetinOzet).HasColumnName("metin_ozet");
            entity.Property(e => e.MetinOzetEn).HasColumnName("metin_ozet_en");
            entity.Property(e => e.OkunmaSayisi).HasColumnName("okunma_sayisi");
            entity.Property(e => e.SirketId).HasColumnName("sirket_id");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("tarih");
            entity.Property(e => e.TurId)
                .HasDefaultValueSql("7")
                .HasColumnName("tur_id");
            entity.Property(e => e.OkunmaSayisi)
                 .HasDefaultValueSql("7")
                 .HasColumnName("okunma_sayisi");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("url");
            entity.Property(e => e.YazarId).HasColumnName("yazar_id");

            entity.HasOne(d => d.Dil).WithMany(p => p.YynKoseYazilaris)
                .HasForeignKey(d => d.DilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kose_yazilari_dilid_fk");

            entity.HasOne(d => d.Sirket).WithMany(p => p.YynKoseYazilaris)
                .HasForeignKey(d => d.SirketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kose_yazilari_sirketid_fk");

            entity.HasOne(d => d.Tur).WithMany(p => p.YynKoseYazilaris)
                .HasForeignKey(d => d.TurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kose_yazilari_turid_fk");

            entity.HasOne(d => d.Yazar).WithMany(p => p.YynKoseYazilaris)
                .HasForeignKey(d => d.YazarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kose_yazilari_yazarid_fk");
        });

        modelBuilder.Entity<YynKoseYazilariKategorileri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_kose_yazilari_kategorileri_pkey");

            entity.ToTable("yyn_kose_yazilari_kategorileri", "yayinlar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KategoriId).HasColumnName("kategori_id");
            entity.Property(e => e.KoseYazisiId).HasColumnName("kose_yazisi_id");
        });

        modelBuilder.Entity<YynTurler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yyn_turler_pkey");

            entity.ToTable("yyn_turler", "yayinlar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adi)
                .HasMaxLength(75)
                .IsFixedLength()
                .HasColumnName("adi");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
