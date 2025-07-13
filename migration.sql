IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Expenditures] (
    [Id] int NOT NULL IDENTITY,
    [Category] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_Expenditures] PRIMARY KEY ([Id])
);

CREATE TABLE [Insurances] (
    [Id] int NOT NULL IDENTITY,
    [Provider] nvarchar(max) NOT NULL,
    [PolicyNumber] nvarchar(max) NOT NULL,
    [CoverageDetails] nvarchar(max) NOT NULL,
    [CoveragedPercentage] float NOT NULL,
    CONSTRAINT [PK_Insurances] PRIMARY KEY ([Id])
);

CREATE TABLE [PartnerSettlements] (
    [Id] int NOT NULL IDENTITY,
    [PartnerName] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [SettlementDate] datetime2 NOT NULL,
    CONSTRAINT [PK_PartnerSettlements] PRIMARY KEY ([Id])
);

CREATE TABLE [PaymentMethod] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY ([Id])
);

CREATE TABLE [RadiologyTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_RadiologyTypes] PRIMARY KEY ([Id])
);

CREATE TABLE [Shifts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [StartTime] time NOT NULL,
    [EndTime] time NOT NULL,
    [TechnicianPercentage] float NOT NULL,
    [IsSpecial] bit NOT NULL,
    CONSTRAINT [PK_Shifts] PRIMARY KEY ([Id])
);

CREATE TABLE [Technicians] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Certification] nvarchar(max) NOT NULL,
    [IsAvailable] bit NOT NULL,
    CONSTRAINT [PK_Technicians] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Patient] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [HasInsurance] bit NOT NULL,
    [InsuranceId] int NULL,
    CONSTRAINT [PK_Patient] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Patient_Insurances_InsuranceId] FOREIGN KEY ([InsuranceId]) REFERENCES [Insurances] ([Id])
);

CREATE TABLE [Reservations] (
    [Id] int NOT NULL IDENTITY,
    [PatientId] int NOT NULL,
    [AppointmentDate] datetime2 NOT NULL,
    [RadiologyTypeId] int NOT NULL,
    [TechnicianId] int NOT NULL,
    [ShiftId] int NOT NULL,
    [BasePrice] decimal(18,2) NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [IsSealed] bit NOT NULL,
    [CoveredByInsurance] bit NOT NULL,
    [TechnicianShare] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reservations_Patient_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patient] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_RadiologyTypes_RadiologyTypeId] FOREIGN KEY ([RadiologyTypeId]) REFERENCES [RadiologyTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Shifts_ShiftId] FOREIGN KEY ([ShiftId]) REFERENCES [Shifts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Technicians_TechnicianId] FOREIGN KEY ([TechnicianId]) REFERENCES [Technicians] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Debts] (
    [Id] int NOT NULL IDENTITY,
    [PatientId] int NULL,
    [ReservationId] int NULL,
    [MobileNumber] nvarchar(max) NOT NULL,
    [Comments] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [IsPaid] bit NOT NULL,
    CONSTRAINT [PK_Debts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Debts_Patient_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patient] ([Id]),
    CONSTRAINT [FK_Debts_Reservations_ReservationId] FOREIGN KEY ([ReservationId]) REFERENCES [Reservations] ([Id])
);

CREATE TABLE [Payment] (
    [Id] int NOT NULL IDENTITY,
    [ReservationId] int NOT NULL,
    [PaymentDate] datetime2 NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [IsCoveredByInsurance] bit NOT NULL,
    [PatientId] int NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payment_Patient_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patient] ([Id]),
    CONSTRAINT [FK_Payment_PaymentMethod_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethod] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_Reservations_ReservationId] FOREIGN KEY ([ReservationId]) REFERENCES [Reservations] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Debts_PatientId] ON [Debts] ([PatientId]);

CREATE INDEX [IX_Debts_ReservationId] ON [Debts] ([ReservationId]);

CREATE INDEX [IX_Patient_InsuranceId] ON [Patient] ([InsuranceId]);

CREATE INDEX [IX_Payment_PatientId] ON [Payment] ([PatientId]);

CREATE INDEX [IX_Payment_PaymentMethodId] ON [Payment] ([PaymentMethodId]);

CREATE INDEX [IX_Payment_ReservationId] ON [Payment] ([ReservationId]);

CREATE INDEX [IX_Reservations_PatientId] ON [Reservations] ([PatientId]);

CREATE INDEX [IX_Reservations_RadiologyTypeId] ON [Reservations] ([RadiologyTypeId]);

CREATE INDEX [IX_Reservations_ShiftId] ON [Reservations] ([ShiftId]);

CREATE INDEX [IX_Reservations_TechnicianId] ON [Reservations] ([TechnicianId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250525100607_first', N'9.0.5');

ALTER TABLE [Debts] DROP CONSTRAINT [FK_Debts_Patient_PatientId];

ALTER TABLE [Patient] DROP CONSTRAINT [FK_Patient_Insurances_InsuranceId];

ALTER TABLE [Payment] DROP CONSTRAINT [FK_Payment_Patient_PatientId];

ALTER TABLE [Reservations] DROP CONSTRAINT [FK_Reservations_Patient_PatientId];

ALTER TABLE [Patient] DROP CONSTRAINT [PK_Patient];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Debts]') AND [c].[name] = N'MobileNumber');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Debts] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Debts] DROP COLUMN [MobileNumber];

EXEC sp_rename N'[Patient]', N'Patients', 'OBJECT';

EXEC sp_rename N'[Patients].[IX_Patient_InsuranceId]', N'IX_Patients_InsuranceId', 'INDEX';

ALTER TABLE [Patients] ADD CONSTRAINT [PK_Patients] PRIMARY KEY ([Id]);

ALTER TABLE [Debts] ADD CONSTRAINT [FK_Debts_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]);

ALTER TABLE [Patients] ADD CONSTRAINT [FK_Patients_Insurances_InsuranceId] FOREIGN KEY ([InsuranceId]) REFERENCES [Insurances] ([Id]);

ALTER TABLE [Payment] ADD CONSTRAINT [FK_Payment_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]);

ALTER TABLE [Reservations] ADD CONSTRAINT [FK_Reservations_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250525102502_sec-aad-patient-to-db', N'9.0.5');

ALTER TABLE [Reservations] ADD [InsuranceId] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250525142111_updaereservation', N'9.0.5');

ALTER TABLE [Reservations] ADD [IsCanceled] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250527083901_up', N'9.0.5');

EXEC sp_rename N'[Reservations].[TotalPrice]', N'PaiedAmount', 'COLUMN';

ALTER TABLE [Reservations] ADD [IsCommission] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Reservations] ADD [IsDebt] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250528095137_NewMigrationName', N'9.0.5');

ALTER TABLE [Debts] ADD [TechnicianId] int NULL;

ALTER TABLE [Debts] ADD [TechnicianShare] decimal(18,2) NOT NULL DEFAULT 0.0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250528111416_debtUpdate', N'9.0.5');

ALTER TABLE [Patients] DROP CONSTRAINT [FK_Patients_Insurances_InsuranceId];

DROP INDEX [IX_Patients_InsuranceId] ON [Patients];

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Insurances]') AND [c].[name] = N'CoverageDetails');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Insurances] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Insurances] DROP COLUMN [CoverageDetails];

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Insurances]') AND [c].[name] = N'CoveragedPercentage');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Insurances] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Insurances] DROP COLUMN [CoveragedPercentage];

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Insurances]') AND [c].[name] = N'Provider');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Insurances] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Insurances] DROP COLUMN [Provider];

ALTER TABLE [Patients] ADD [InsuranceId1] int NOT NULL DEFAULT 0;

ALTER TABLE [Insurances] ADD [InsuranceAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Insurances] ADD [IsComplete] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Insurances] ADD [PaidAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Insurances] ADD [PatientId] int NULL;

ALTER TABLE [Insurances] ADD [ProviderId] int NULL;

ALTER TABLE [Insurances] ADD [ReservationId] int NULL;

ALTER TABLE [Insurances] ADD [TechnicianId] int NULL;

ALTER TABLE [Insurances] ADD [TechnicianShare] decimal(18,2) NOT NULL DEFAULT 0.0;

CREATE TABLE [InsuranceCompanies] (
    [Id] int NOT NULL IDENTITY,
    [Provider] nvarchar(max) NOT NULL,
    [PolicyNumber] nvarchar(max) NOT NULL,
    [CoverageDetails] nvarchar(max) NOT NULL,
    [CoveragedPercentage] float NOT NULL,
    CONSTRAINT [PK_InsuranceCompanies] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_Patients_InsuranceId1] ON [Patients] ([InsuranceId1]);

ALTER TABLE [Patients] ADD CONSTRAINT [FK_Patients_Insurances_InsuranceId1] FOREIGN KEY ([InsuranceId1]) REFERENCES [Insurances] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615145307_InitialCreate', N'9.0.5');

ALTER TABLE [Patients] DROP CONSTRAINT [FK_Patients_Insurances_InsuranceId1];

DROP INDEX [IX_Patients_InsuranceId1] ON [Patients];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Patients]') AND [c].[name] = N'InsuranceId1');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Patients] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Patients] DROP COLUMN [InsuranceId1];

CREATE INDEX [IX_Patients_InsuranceId] ON [Patients] ([InsuranceId]);

ALTER TABLE [Patients] ADD CONSTRAINT [FK_Patients_InsuranceCompanies_InsuranceId] FOREIGN KEY ([InsuranceId]) REFERENCES [InsuranceCompanies] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615150927_Innsurance', N'9.0.5');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250615151311_Innsurance2', N'9.0.5');

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Expenditures]') AND [c].[name] = N'Date');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Expenditures] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Expenditures] DROP COLUMN [Date];

EXEC sp_rename N'[PartnerSettlements].[Amount]', N'Amount_Percentage', 'COLUMN';

CREATE TABLE [TechnicianSettlements] (
    [Id] int NOT NULL IDENTITY,
    [TechnicianId] int NOT NULL,
    [TechnicianName] nvarchar(max) NOT NULL,
    [TotalFromReservations] decimal(18,2) NOT NULL,
    [TotalFromInsurance] decimal(18,2) NOT NULL,
    [TotalDebtShare] decimal(18,2) NOT NULL,
    [TotalExpenses] decimal(18,2) NOT NULL,
    [NetPayable] decimal(18,2) NOT NULL,
    [GeneratedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_TechnicianSettlements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TechnicianSettlements_Technicians_TechnicianId] FOREIGN KEY ([TechnicianId]) REFERENCES [Technicians] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [TechnicianSettlementViewModel] (
    [Id] int NOT NULL IDENTITY,
    [TechnicianName] nvarchar(max) NOT NULL,
    [TotalFromReservations] decimal(18,2) NOT NULL,
    [TotalFromInsurance] decimal(18,2) NOT NULL,
    [TotalDebtShare] decimal(18,2) NOT NULL,
    [TotalExpenses] decimal(18,2) NOT NULL,
    [NetPayable] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_TechnicianSettlementViewModel] PRIMARY KEY ([Id])
);

CREATE TABLE [InsuranceBreakdown] (
    [Id] int NOT NULL IDENTITY,
    [InsuranceCompany] nvarchar(max) NOT NULL,
    [TechnicianShare] decimal(18,2) NOT NULL,
    [TechnicianSettlementViewModelId] int NULL,
    CONSTRAINT [PK_InsuranceBreakdown] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InsuranceBreakdown_TechnicianSettlementViewModel_TechnicianSettlementViewModelId] FOREIGN KEY ([TechnicianSettlementViewModelId]) REFERENCES [TechnicianSettlementViewModel] ([Id])
);

CREATE INDEX [IX_InsuranceBreakdown_TechnicianSettlementViewModelId] ON [InsuranceBreakdown] ([TechnicianSettlementViewModelId]);

CREATE INDEX [IX_TechnicianSettlements_TechnicianId] ON [TechnicianSettlements] ([TechnicianId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250621190026_Dates', N'9.0.5');

ALTER TABLE [TechnicianSettlementViewModel] ADD [SettelmentDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Insurances] ADD [IsTechnicianShared] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Debts] ADD [IsTechnicianShared] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250623084941_IsTechShared', N'9.0.5');

EXEC sp_rename N'[PartnerSettlements].[Amount_Percentage]', N'Amount', 'COLUMN';

ALTER TABLE [Insurances] ADD [IsSealed] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Expenditures] ADD [IsSealed] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Debts] ADD [IsSealed] bit NOT NULL DEFAULT CAST(0 AS bit);

CREATE TABLE [InsuranceCompanySettlementViewModel] (
    [Id] int NOT NULL IDENTITY,
    [InsuranceCompanyName] nvarchar(max) NOT NULL,
    [TotalInsuranceShare] decimal(18,2) NOT NULL,
    [NetPayable] decimal(18,2) NOT NULL,
    [SettlementDate] datetime2 NOT NULL,
    CONSTRAINT [PK_InsuranceCompanySettlementViewModel] PRIMARY KEY ([Id])
);

CREATE TABLE [Partners] (
    [Id] int NOT NULL IDENTITY,
    [PartnerName] nvarchar(max) NOT NULL,
    [Amount_Percentage] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Partners] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250624090133_Parteners', N'9.0.5');

DROP TABLE [InsuranceBreakdown];

DROP TABLE [InsuranceCompanySettlementViewModel];

DROP TABLE [TechnicianSettlementViewModel];

ALTER TABLE [TechnicianSettlements] ADD [SettelmentDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

CREATE TABLE [InsuranceCompanySettlements] (
    [Id] int NOT NULL IDENTITY,
    [InsuranceCompanyName] nvarchar(max) NOT NULL,
    [TotalInsuranceShare] decimal(18,2) NOT NULL,
    [NetPayable] decimal(18,2) NOT NULL,
    [SettlementDate] datetime2 NOT NULL,
    CONSTRAINT [PK_InsuranceCompanySettlements] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250626075337_pendingissues', N'9.0.5');

ALTER TABLE [Reservations] ADD [IsTechnicianShared] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250626091937_addtechsharedtoreservationclass', N'9.0.5');

ALTER TABLE [InsuranceCompanySettlements] ADD [ProviderId] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250626102339_new', N'9.0.5');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629113750_newdb', N'9.0.5');

ALTER TABLE [Patients] DROP CONSTRAINT [FK_Patients_InsuranceCompanies_InsuranceId];

ALTER TABLE [Payment] DROP CONSTRAINT [FK_Payment_Patients_PatientId];

DROP INDEX [IX_Payment_PatientId] ON [Payment];

DROP INDEX [IX_Patients_InsuranceId] ON [Patients];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payment]') AND [c].[name] = N'PatientId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Payment] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Payment] DROP COLUMN [PatientId];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Patients]') AND [c].[name] = N'HasInsurance');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Patients] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Patients] DROP COLUMN [HasInsurance];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Patients]') AND [c].[name] = N'InsuranceId');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Patients] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Patients] DROP COLUMN [InsuranceId];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629115424_updatepatient', N'9.0.5');

ALTER TABLE [Insurances] ADD [IsCanceled] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Debts] ADD [IsCanceled] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629125122_updatepatient-aadcancelflag', N'9.0.5');

COMMIT;
GO

