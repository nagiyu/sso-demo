-- データベース初期化時以外は実行されないので、適用は下記コマンドで
-- docker exec sso-demo_postgres psql -U postgres -d sso_demo_db -f /docker-entrypoint-initdb.d/init.sql

CREATE TABLE "AspNetRoles" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "ConcurrencyStamp" TEXT,
    "Name" VARCHAR(256),
    "NormalizedName" VARCHAR(256),
    CONSTRAINT "RoleNameIndex" UNIQUE ("NormalizedName")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    "RoleId" VARCHAR(450) NOT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUsers" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "AccessFailedCount" INTEGER NOT NULL,
    "ConcurrencyStamp" TEXT,
    "Email" VARCHAR(256),
    "EmailConfirmed" BOOLEAN NOT NULL,
    "LockoutEnabled" BOOLEAN NOT NULL,
    "LockoutEnd" TIMESTAMPTZ,
    "NormalizedEmail" VARCHAR(256),
    "NormalizedUserName" VARCHAR(256),
    "PasswordHash" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL,
    "SecurityStamp" TEXT,
    "TwoFactorEnabled" BOOLEAN NOT NULL,
    "UserName" VARCHAR(256),
    CONSTRAINT "UserNameIndex" UNIQUE ("NormalizedUserName"),
    CONSTRAINT "EmailIndex" UNIQUE ("NormalizedEmail")
);

CREATE TABLE "AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    "UserId" VARCHAR(450) NOT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" VARCHAR(128) NOT NULL,
    "ProviderKey" VARCHAR(128) NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" VARCHAR(450) NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" VARCHAR(450) NOT NULL,
    "RoleId" VARCHAR(450) NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" VARCHAR(450) NOT NULL,
    "LoginProvider" VARCHAR(128) NOT NULL,
    "Name" VARCHAR(128) NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE
);
