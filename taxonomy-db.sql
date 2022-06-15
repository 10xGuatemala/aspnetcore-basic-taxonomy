
-- ----------------------------------------------------------------------------
-- User and Dabase creation.
-- First step
-- ----------------------------------------------------------------------------

create user bootcamp with password 'yourDevPass123$';
create user bootcamp_app with password 'yourDevPass123$';
create database bootcamps with owner bootcamp encoding 'UTF8';
revoke all privileges on database bootcamps from public;
grant connect on database bootcamps to bootcamp;
grant connect on database bootcamps to bootcamp_app;


-- ------------------------------------------------------------------------------------
-- DB USER: bootcamp
-- Creation of schema, objects and assignment of permissions defaults for taxonomy
-- ------------------------------------------------------------------------------------

-- 1) schema creation
create schema taxonomy;

-- 2) The schema is included in the search path
alter database bootcamps set search_path to taxonomy, public;

-- 3) default DML permissions are assigned to the application user

grant usage on schema taxonomy to bootcamp_app;
alter default privileges in schema taxonomy grant select, insert, update, delete on tables to bootcamp_app;
alter default privileges in schema taxonomy grant select, usage on sequences to bootcamp_app;

-- 4) instructions for creating tables/views/secuences

create table taxonomy.audit (
  audit_id            serial not null,
  created_by          varchar(20) not null,
  created_on        date not null,
  modified_by         varchar(20),
  modified_on       date,
  primary key (audit_id)
);

create table taxonomy.family (
  family_id         integer not null default nextval('taxonomy.family_seq'),
  family_name              varchar(50) not null,
  primary key (family_id)
)inherits (taxonomy.audit);

create table taxonomy.genus (
  family_id         integer not null,
  genus_id          integer not null,
  genus_name              varchar(50) not null,
  primary key (family_id, genus_id),
  foreign key (family_id) REFERENCES taxonomy.family (family_id)
)inherits (taxonomy.audit);

create table taxonomy.species (
  family_id         integer not null,
  genus_id          integer not null,
  specie_id         integer not null,
  specie_name              varchar(50) not null,
  primary key (family_id, genus_id, specie_id),
  foreign key (family_id, genus_id) REFERENCES taxonomy.genus (family_id, genus_id)
)inherits (taxonomy.audit);

create or replace view taxonomy.datamart as
select t1.family_id,
       t1.family_name,
       t2.genus_id,
       t2.genus_name,
       t3.specie_id,
       t3.specie_name
from taxonomy.family t1
     left outer join taxonomy.genus t2 on (t2.family_id = t1.family_id)
     left outer join taxonomy.species t3 on (t3.genus_id = t2.genus_id  and t3.family_id = t2.family_id);

create sequence taxonomy.family_seq increment by 1000 minvalue 1000 maxvalue 100000 start 1000;
create sequence taxonomy.genus_seq increment by 1 minvalue 100 maxvalue 100000 start 100;
create sequence taxonomy.species_seq increment by 1 minvalue 100 maxvalue 100000 start 100;



