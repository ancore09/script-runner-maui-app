create table if not exists users
(
    id       serial
    constraint users_pk
    primary key,
    username text not null
);

alter table users
    owner to runner;

create unique index if not exists users_id_uindex
    on users (id);

create unique index if not exists users_username_uindex
    on users (username);

create table if not exists credentials
(
    id       serial
    constraint credentials_pk
    primary key,
    username text not null,
    password text not null
);

alter table credentials
    owner to runner;

create unique index if not exists credentials_id_uindex
    on credentials (id);

create unique index if not exists credentials_username_uindex
    on credentials (username);

create table if not exists scripts
(
    id          serial
    constraint scripts_pk
    primary key,
    name        text not null,
    description text not null,
    args        text default ''::text,
    path        text
);

alter table scripts
    owner to runner;

create unique index if not exists scripts_id_uindex
    on scripts (id);

create table if not exists user_scripts
(
    user_id   integer not null
    references users
    on delete cascade,
    script_id integer not null
    references scripts
    on delete cascade,
    primary key (user_id, script_id)
    );

alter table user_scripts
    owner to runner;

