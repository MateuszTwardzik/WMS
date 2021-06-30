PGDMP         *                y           MAGAZYN    13.2    13.2     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
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
    "Price" numeric NOT NULL,
    "TypeId" integer
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
       public          postgres    false    202            �           0    0    Product_Id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."Product_Id_seq" OWNED BY public."Product"."Id";
          public          postgres    false    201            �            1259    24850    Product_Type    TABLE     \   CREATE TABLE public."Product_Type" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);
 "   DROP TABLE public."Product_Type";
       public         heap    postgres    false            �            1259    24848    Product_Type_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."Product_Type_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public."Product_Type_Id_seq";
       public          postgres    false    206            �           0    0    Product_Type_Id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public."Product_Type_Id_seq" OWNED BY public."Product_Type"."Id";
          public          postgres    false    205            �            1259    24839    User    TABLE     �   CREATE TABLE public."User" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Password" text NOT NULL,
    "Permission" integer NOT NULL
);
    DROP TABLE public."User";
       public         heap    postgres    false            �            1259    24837    User_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."User_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public."User_Id_seq";
       public          postgres    false    204            �           0    0    User_Id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public."User_Id_seq" OWNED BY public."User"."Id";
          public          postgres    false    203            �            1259    24598    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false            5           2604    24638 
   Product Id    DEFAULT     n   ALTER TABLE ONLY public."Product" ALTER COLUMN "Id" SET DEFAULT nextval('public."Product_Id_seq"'::regclass);
 =   ALTER TABLE public."Product" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    202    201    202            7           2604    24853    Product_Type Id    DEFAULT     x   ALTER TABLE ONLY public."Product_Type" ALTER COLUMN "Id" SET DEFAULT nextval('public."Product_Type_Id_seq"'::regclass);
 B   ALTER TABLE public."Product_Type" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    205    206    206            6           2604    24842    User Id    DEFAULT     h   ALTER TABLE ONLY public."User" ALTER COLUMN "Id" SET DEFAULT nextval('public."User_Id_seq"'::regclass);
 :   ALTER TABLE public."User" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    203    204    204            �          0    24635    Product 
   TABLE DATA                 public          postgres    false    202   �       �          0    24850    Product_Type 
   TABLE DATA                 public          postgres    false    206   2       �          0    24839    User 
   TABLE DATA                 public          postgres    false    204   �       �          0    24598    __EFMigrationsHistory 
   TABLE DATA                 public          postgres    false    200   B        �           0    0    Product_Id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Product_Id_seq"', 42, true);
          public          postgres    false    201            �           0    0    Product_Type_Id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."Product_Type_Id_seq"', 2, true);
          public          postgres    false    205            �           0    0    User_Id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public."User_Id_seq"', 4, true);
          public          postgres    false    203            9           2606    24602 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    200            ?           2606    24858    Product_Type Product_Type_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."Product_Type"
    ADD CONSTRAINT "Product_Type_pkey" PRIMARY KEY ("Id");
 L   ALTER TABLE ONLY public."Product_Type" DROP CONSTRAINT "Product_Type_pkey";
       public            postgres    false    206            ;           2606    24643    Product Product_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Product_pkey" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Product" DROP CONSTRAINT "Product_pkey";
       public            postgres    false    202            =           2606    24847    User User_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
       public            postgres    false    204            @           2606    24859    Product Type    FK CONSTRAINT     �   ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Type" FOREIGN KEY ("TypeId") REFERENCES public."Product_Type"("Id") NOT VALID;
 :   ALTER TABLE ONLY public."Product" DROP CONSTRAINT "Type";
       public          postgres    false    2879    202    206            �   �  x���Qn�@��=ņĠ	!,��i} RK�6�.B�!Fn�s���:��7�!������l��,�[��o��eu����Qy�o61�ܰ������<�e{��)����z*�ؔ}Γ�ņM��b�s_T���4-�9�(��>�b�ė�W�j�< }��f
��Ȧ��@�h�ul��D���9��4���;��v	�q��.+*�Y,԰�Ci3��vs&
$��%Q��ϥ����O���#|-�F}�Z��>)�#���3o���x���=�b��(�t���B��T�b�Syn%����3p�0N�S�8���E%�c���9Z�Bes��EϺ�͐4�=��Zu6"�C�~�~�7Y�k��=E�_�J����R���7������/8      �   �   x���v
Q���W((M��L�S
(�O)M.��,HUR�P�LQ�QP�K�MU�Ts�	uV�0�QP?���</���#���\��e
4�9#573�"c̀�x��Rd�1�Gw��0�ᛚ�v	 �f�      �   p   x���v
Q���W((M��L�S
-N-RR�P�LQ�QP�K�M������E`��Ԣ������<%M�0G�P�`C�Ĕ��<u$����5�'�,1�]
Ԫ���6pq �8�      �   
   x���         