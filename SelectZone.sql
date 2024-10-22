PGDMP      '                |         
   SelectZone    16.3    16.3 
    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    237582 
   SelectZone    DATABASE        CREATE DATABASE "SelectZone" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';
    DROP DATABASE "SelectZone";
                postgres    false            �            1255    245774 3   retreivefiles(timestamp without time zone, integer)    FUNCTION     p  CREATE FUNCTION public.retreivefiles(date timestamp without time zone, processtype integer DEFAULT 0) RETURNS TABLE(amdid integer, docpath character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
     if processtype = 1 THEN
	  RETURN QUERY
	  SELECT
      u.amdid
       ,u.docpath
	  from public.szfilemaster u
	WHERE u.recordinserteddate = date;
	  END IF;
END;
$$;
 [   DROP FUNCTION public.retreivefiles(date timestamp without time zone, processtype integer);
       public          postgres    false            �            1255    237591 :   usp_insert_filemaster(character varying, integer, integer) 	   PROCEDURE     �  CREATE PROCEDURE public.usp_insert_filemaster(OUT p_outputparam character varying, IN p_docpath character varying, IN p_amdid integer DEFAULT 0, IN processtype integer DEFAULT 0)
    LANGUAGE plpgsql
    AS $$
DECLARE docaddress TEXT;
BEGIN
	FOREACH docaddress IN Array string_to_array(p_docpath,',')
		loop
			 IF docaddress IS NOT NULL AND docaddress <> '' THEN
	     		SELECT COALESCE(MAX(amdid), 0) + 1 INTO p_amdid FROM public.szfilemaster;
			INSERT INTO public.szfilemaster
				(
					amdid,
					docpath,
					recordinserteddate
				)
				Values
				(
					p_amdid,
					docaddress,
					Now()
				);
			end if;
		end loop;
			 p_outputparam := 'Done';
END;
$$;
 �   DROP PROCEDURE public.usp_insert_filemaster(OUT p_outputparam character varying, IN p_docpath character varying, IN p_amdid integer, IN processtype integer);
       public          postgres    false            �            1255    253966 (   usp_update_file_status(integer, integer) 	   PROCEDURE     Y  CREATE PROCEDURE public.usp_update_file_status(OUT outputparam character varying, IN processtype integer, IN p_amdid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF processtype = 1 THEN
	
       BEGIN
			delete from public.szfilemaster
			WHERE amdid = p_amdid;
        	outputparam := 'File Removed Successfully';
		end;
	END IF;
END;
$$;
 }   DROP PROCEDURE public.usp_update_file_status(OUT outputparam character varying, IN processtype integer, IN p_amdid integer);
       public          postgres    false            �            1259    237583    szfilemaster    TABLE     }   CREATE TABLE public.szfilemaster (
    amdid integer NOT NULL,
    docpath character varying,
    recordinserteddate date
);
     DROP TABLE public.szfilemaster;
       public         heap    postgres    false            �          0    237583    szfilemaster 
   TABLE DATA           J   COPY public.szfilemaster (amdid, docpath, recordinserteddate) FROM stdin;
    public          postgres    false    215   �       S           2606    237589    szfilemaster szfilemaster_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public.szfilemaster
    ADD CONSTRAINT szfilemaster_pkey PRIMARY KEY (amdid);
 H   ALTER TABLE ONLY public.szfilemaster DROP CONSTRAINT szfilemaster_pkey;
       public            postgres    false    215            �   l  x�ŕMk1���_���0F}��&-��4�B1}�'��MI�}头"��\vZއ��3�G����s��8~�nh>��]����M�~�����h8N��f>����>����O�pM�j��O�6c?�a�M}���çпo'�����f ]'^�X��W`�2`Y��k�ؼV��k��5�S�r�iU����w�`g�i��_Ҿ���ޘ�~��T�J+��R� Y��q��<ڔt:�<I�x���;�j(*�� B[��KQg����SJ��J+T`���ϴó	����kiu/J�ɃU��ڃ�Q�W��2|Mi͆�D�
�� R�6���{�ZהV��ю����� �q�t�@����Ҫ���MAJ/!A�q����>��%� ��s��* ���-�g�Z�U<
�vSLdN
�+�fJa���Cl�DT2���r��96F�˄�9Q��V�?��-.Nf��_���lQ�p�.���ପh����:���o�~��v��a��Q��~��0Qb�������������/���O���:ڨ�u6|������������_���h5��U���V�C�|�u�O���     