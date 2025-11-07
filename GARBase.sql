CREATE TABLE IF NOT EXISTS addr_obj_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);

CREATE table IF NOT EXISTS address_objects (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    objectguid UUID NOT NULL,
    changeid BIGINT NOT NULL,
    name VARCHAR NOT NULL,
    typename VARCHAR NOT NULL,
    level VARCHAR NOT NULL,
    opertypeid INTEGER NOT NULL,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL,
    isactive BOOLEAN NOT NULL
);


CREATE TABLE IF NOT EXISTS address_object_division (
    id BIGINT PRIMARY KEY,
    parentid BIGINT NOT NULL,
    childid BIGINT NOT NULL,
    changeid BIGINT NOT NULL
);

CREATE TABLE IF NOT EXISTS address_object_types (
    id INTEGER PRIMARY KEY,
    level INTEGER NOT NULL,
    shortname VARCHAR(50) NOT NULL,
    name VARCHAR NOT NULL,
    description VARCHAR,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS adm_hierarchy (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    parentobjid BIGINT,
    changeid BIGINT NOT NULL,
    regioncode VARCHAR(255),
    areacode VARCHAR(255),
    citycode VARCHAR(255),
    placecode VARCHAR(255),
    plancode VARCHAR(255),
    streetcode VARCHAR(255),
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL,
    path TEXT
);

CREATE TABLE IF NOT EXISTS apartments_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS apartment_type (
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    shortname VARCHAR(50),
    description TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS apartments (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    objectguid UUID NOT NULL,
    changeid BIGINT NOT NULL,
    numberapart VARCHAR,
    aparttype INTEGER NOT NULL,
    opertypeid BIGINT NOT NULL,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS car_place (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    objectguid UUID NOT NULL,
    changeid BIGINT NOT NULL,
    placenumber VARCHAR,
    opertypeid INTEGER NOT NULL,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS carplaces_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS change_history_item (
    changeid BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    adrobjectid UUID NOT NULL,
    opertypeid INTEGER NOT NULL,
    ndocid BIGINT NULL,
    changedate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS house_types (
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    shortname VARCHAR(50) NOT NULL,
    description TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL    
);

CREATE TABLE IF NOT EXISTS houses (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    objectguid UUID NOT NULL,
    changeid BIGINT NOT NULL,
    housenum VARCHAR,
    addnum1 VARCHAR,
    addnum2 VARCHAR,
    housetype INTEGER NOT NULL,
    addtype1 INTEGER NOT NULL,
    addtype2 INTEGER NOT NULL,
    opertypeid INTEGER NOT NULL,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS houses_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS mun_hierarchy (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    parentobjid BIGINT,
    changeid BIGINT NOT NULL,
    oktmo VARCHAR,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL,
    path VARCHAR
);

CREATE TABLE IF NOT EXISTS normative_doc (
    id BIGINT PRIMARY KEY,
    name VARCHAR NOT NULL,
    doc_date DATE NOT NULL,
    doc_number VARCHAR,
    type INTEGER NOT NULL,
    kind INTEGER NOT NULL,
    update_date DATE NOT NULL,
    org_name VARCHAR,
    reg_num VARCHAR,
    reg_date DATE,
    acc_date DATE,
    comment TEXT
);

CREATE TABLE IF NOT EXISTS normative_doc_kind(
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS normative_doc_types (
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS object_levels (
    level INTEGER NOT NULL,
    name VARCHAR NOT NULL,
    shortname VARCHAR NOT NULL,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS operation_type (
    id INTEGER PRIMARY KEY,
    name VARCHAR NOT NULL,
    shortname VARCHAR NOT NULL,
    description TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS param_types (
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    code VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS reestr_object (
    objectid BIGINT NOT NULL,
    createdate DATE NOT NULL,
    changeid BIGINT NOT NULL,
    levelid BIGINT NOT NULL,
    updatedate DATE NOT NULL,
    objectguid UUID NOT NULL,
    isactive BOOLEAN NOT null,
    PRIMARY KEY (OBJECTID, CHANGEID)
);

CREATE TABLE IF NOT EXISTS room (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    objectguid UUID NOT NULL,
    changeid BIGINT NOT NULL,
    roomnumber VARCHAR NOT NULL,
    roomtype INTEGER NOT NULL,
    opertypeid INTEGER NOT NULL,
    previd BIGINT,
    nextid BIGINT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS room_type (
    id INTEGER PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    shortname VARCHAR(50) NOT NULL,
    description TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactive BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS rooms_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS stead (
    id INTEGER PRIMARY KEY,
    objectid INTEGER NOT NULL,
    objectguid UUID NOT NULL,
    changeid INTEGER NOT NULL,
    steadnumber VARCHAR NOT NULL,
    opertypeid VARCHAR NOT NULL,
    previd INTEGER NOT NULL,
    nextid INTEGER NOT NULL,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL,
    isactual BOOLEAN NOT NULL DEFAULT true,
    isactive BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS steads_params (
    id BIGINT PRIMARY KEY,
    objectid BIGINT NOT NULL,
    changeid BIGINT,
    changeidend BIGINT NOT NULL,
    typeid INTEGER NOT NULL,
    value TEXT,
    updatedate DATE NOT NULL,
    startdate DATE NOT NULL,
    enddate DATE NOT NULL
);