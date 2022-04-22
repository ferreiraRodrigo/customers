--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2 (Debian 14.2-1.pgdg110+1)
-- Dumped by pg_dump version 14.2

-- Started on 2022-04-22 04:05:55 UTC

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 210 (class 1259 OID 16390)
-- Name: Customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Customers" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text NOT NULL,
    "Scopes" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Customers" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16407)
-- Name: Products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Products" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Title" text,
    "Price" real NOT NULL,
    "Image" text,
    "WishListId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."Products" OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16397)
-- Name: WishLists; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."WishLists" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "DeletedAt" timestamp with time zone
);


ALTER TABLE public."WishLists" OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 16385)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 3330 (class 0 OID 16390)
-- Dependencies: 210
-- Data for Name: Customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Customers" ("Id", "Name", "Email", "Password", "Scopes", "CreatedAt", "UpdatedAt", "DeletedAt") FROM stdin;
\.


--
-- TOC entry 3332 (class 0 OID 16407)
-- Dependencies: 212
-- Data for Name: Products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Products" ("Id", "ProductId", "Title", "Price", "Image", "WishListId", "CreatedAt", "UpdatedAt", "DeletedAt") FROM stdin;
\.


--
-- TOC entry 3331 (class 0 OID 16397)
-- Dependencies: 211
-- Data for Name: WishLists; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."WishLists" ("Id", "CustomerId", "CreatedAt", "UpdatedAt", "DeletedAt") FROM stdin;
\.


--
-- TOC entry 3329 (class 0 OID 16385)
-- Dependencies: 209
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20220422034046_InitialMigration	5.0.0
\.


--
-- TOC entry 3181 (class 2606 OID 16396)
-- Name: Customers PK_Customers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Customers"
    ADD CONSTRAINT "PK_Customers" PRIMARY KEY ("Id");


--
-- TOC entry 3187 (class 2606 OID 16413)
-- Name: Products PK_Products; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "PK_Products" PRIMARY KEY ("Id");


--
-- TOC entry 3184 (class 2606 OID 16401)
-- Name: WishLists PK_WishLists; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WishLists"
    ADD CONSTRAINT "PK_WishLists" PRIMARY KEY ("Id");


--
-- TOC entry 3179 (class 2606 OID 16389)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3185 (class 1259 OID 16419)
-- Name: IX_Products_WishListId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Products_WishListId" ON public."Products" USING btree ("WishListId");


--
-- TOC entry 3182 (class 1259 OID 16420)
-- Name: IX_WishLists_CustomerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_WishLists_CustomerId" ON public."WishLists" USING btree ("CustomerId");


--
-- TOC entry 3189 (class 2606 OID 16414)
-- Name: Products FK_Products_WishLists_WishListId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "FK_Products_WishLists_WishListId" FOREIGN KEY ("WishListId") REFERENCES public."WishLists"("Id") ON DELETE CASCADE;


--
-- TOC entry 3188 (class 2606 OID 16402)
-- Name: WishLists FK_WishLists_Customers_CustomerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."WishLists"
    ADD CONSTRAINT "FK_WishLists_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES public."Customers"("Id") ON DELETE CASCADE;


-- Completed on 2022-04-22 04:05:56 UTC

--
-- PostgreSQL database dump complete
--

