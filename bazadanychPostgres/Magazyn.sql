PGDMP     9                    y           MAGAZYN    13.2    13.2     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    24576    MAGAZYN    DATABASE     e   CREATE DATABASE "MAGAZYN" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Polish_Poland.1250';
    DROP DATABASE "MAGAZYN";
                postgres    false            �            1259    24635    Product    TABLE     �   CREATE TABLE public."Product" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Quantity" integer NOT NULL,
    "Price" numeric NOT NULL
);
    DROP TABLE public."Product";
       public         heap    postgres    false            �            1259    24633    Product_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."Product_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public."Product_Id_seq";
       public          postgres    false    203            �           0    0    Product_Id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."Product_Id_seq" OWNED BY public."Product"."Id";
          public          postgres    false    202            �            1259    24585    User    TABLE     �   CREATE TABLE public."User" (
    "Id" integer NOT NULL,
    "Name" name NOT NULL,
    "Password" name NOT NULL,
    "Permission" integer
);
    DROP TABLE public."User";
       public         heap    postgres    false            �            1259    24598    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false            +           2604    24638 
   Product Id    DEFAULT     n   ALTER TABLE ONLY public."Product" ALTER COLUMN "Id" SET DEFAULT nextval('public."Product_Id_seq"'::regclass);
 =   ALTER TABLE public."Product" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    203    202    203            �          0    24635    Product 
   TABLE DATA           F   COPY public."Product" ("Id", "Name", "Quantity", "Price") FROM stdin;
    public          postgres    false    203   �       �          0    24585    User 
   TABLE DATA           H   COPY public."User" ("Id", "Name", "Password", "Permission") FROM stdin;
    public          postgres    false    200   3       �          0    24598    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    201   e       �           0    0    Product_Id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Product_Id_seq"', 21, true);
          public          postgres    false    202            -           2606    24589    User Client_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "Client_pkey" PRIMARY KEY ("Id");
 >   ALTER TABLE ONLY public."User" DROP CONSTRAINT "Client_pkey";
       public            postgres    false    200            /           2606    24602 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    201            1           2606    24643    Product Product_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Product_pkey" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Product" DROP CONSTRAINT "Product_pkey";
       public            postgres    false    203            �   �   x�5�1�0����`a%W�aM��E�#a0E0DB¯�
8ޓ�_���E`��SFd���^��.p���vOx{�4ME��ŭ��VC����+rV����k����w_ъ��-B-T�,k�e��^y�q��%�yfƘa`-�      �   "   x�3�LL��̃���\F��ũE+F��� �	�      �      x������ � �     