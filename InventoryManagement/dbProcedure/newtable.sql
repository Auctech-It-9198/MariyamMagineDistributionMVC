use Maryamsoft
go
-------------------------------------------------------Users
create table tbl_users
(
userid int not null,
username varchar(100) ,
passw varchar(50),
logintype smallint,
registered datetime,
Latest_Activity datetime,
IsLocked char(1),
loginCnt int,
ustatus bit
)
go
----------------------------------------------------------------------
alter table tbl_users add constraint pk_tbl_users primary key (userid)
go
----------------------------------------------------------------------
alter table tbl_users add constraint uk_tbl_users unique (username)
go
----===================================================================
create table tbl_company 
(
compcode tinyint not null,
cmpname varchar(100) not null,
address1 varchar(100) not null,
address2 varchar(100),
gstin varchar(15),
panno varchar(15),
mobileno varchar(50),
email varchar(100),
website varchar(100),
footer1 varchar(500),
footer2 varchar(500),
footer3 varchar(500),
footer4 varchar(500),
footer5 varchar(500)
)
go
----------------------------------------------------------------------
alter table tbl_company add constraint pk_tbl_company primary key (compcode)
go
----------------------------------------------------------------------
alter table tbl_company add constraint uk_tbl_company unique (cmpname,gstin)
go
----===================================================================
create table tbl_compusers
(
id int identity(1,1),
userid int not null,
compcode tinyint not null,
)
go
----------------------------------------------------------------------
alter table tbl_compusers add constraint pk_tbl_compusers primary key (id)
go
----------------------------------------------------------------------
alter table tbl_compusers add constraint uk_tbl_compusers unique (userid,compcode)
go
----------------------------------------------------------------------
alter table tbl_compusers add constraint fk_tbl_compusers_tbl_company
foreign key (compcode) references tbl_company(compcode)
go
----===================================================================
create table tbl_menu(
mid int IDENTITY(1,1) NOT NULL,
mname varchar(50) NULL,
li_class varchar(50) NULL,
a_class varchar(50) NULL,
url varchar(50) NULL,
fa_icon varchar(200) NULL,
arrow varchar(50) NULL,
morder int,
compcode tinyint
) 
go
----------------------------------------------------------------------
alter table tbl_menu add constraint pk_tbl_menu primary key (mid)
go
----------------------------------------------------------------------
alter table tbl_menu add constraint uk_tbl_menu unique (mname,compcode)
go
----------------------------------------------------------------------
alter table tbl_menu add constraint fk_tbl_menu_tbl_company
foreign key (compcode) references tbl_company(compcode)
go
----===================================================================
create table tbl_smenu
(
smid int identity(1,1),
mid int,
smname varchar(100),
li_class varchar(50) NULL,
a_class varchar(50) NULL,
url varchar(50) NULL,
fa_icon varchar(200) NULL,
arrow varchar(50) NULL,
smorder int,
compcode tinyint
)
go
----------------------------------------------------------------------
alter table tbl_smenu add constraint pk_tbl_smenu primary key (smid)
go
----------------------------------------------------------------------
alter table tbl_smenu add constraint uk_tbl_smenu unique (mid,smname,compcode)
go
----------------------------------------------------------------------
alter table tbl_smenu add constraint fk_tbl_smenu_tbl_company
foreign key (compcode) references tbl_company(compcode)
go
----------------------------------------------------------------------
alter table tbl_smenu add constraint fk_tbl_smenu_tbl_menu
foreign key (mid) references tbl_menu(mid)
go
----==================================================================
create table tbl_roles
(
id int IDENTITY(1,1) NOT NULL,
compcode tinyint,
mid int,
userid int,
isenabled bit
)
go
----==================================================================
create table tbl_permissions
(
id int IDENTITY(1,1) NOT NULL,
compcode tinyint,
smid int,
userid int,
isenabled bit
)
go
----==================================================================
create table tbl_menu_permission
(
id int identity(1,1) constraint pk_tbl_menu_permission primary key,
compcode tinyint constraint fk_ttbl_menu_permission_tbl_company foreign key references tbl_company(compcode),
userid int constraint fk_tbl_menu_permission_tbl_users foreign key references tbl_users(userid),
mid int,
menuper bit,--{(Yes/No)-(1/0)}--menu permission
approve bit
constraint uk_tbl_menu_permission unique(compcode,userid,mid)
)
go
----==================================================================
create table tbl_userpermission
(
id int identity(1,1) constraint pk_tbl_userpermission primary key,
compcode tinyint constraint fk_tbl_userpermission_tbl_company foreign key references tbl_company(compcode),
userid int constraint fk_tbl_userpermission_tbl_users foreign key references tbl_users(userid),
mid int,
smid int,--referred by enum object (Entry)
addper bit,--{(Yes/No)-(1/0)}--add permission
editper bit,--{(Yes/No)-(1/0)}--edit permission
deleteper bit,--{(Yes/No)-(1/0)}--delete permission
viewper bit,--{(Yes/No)-(1/0)}--view permission
printper bit,--{(Yes/No)-(1/0)}--print permission
menuper bit--{(Yes/No)-(1/0)}--menu permission
constraint uk_tbl_userpermission unique(compcode,userid,smid)
)
go
------------------------------------------------------Country
create table tbl_country
(
cid int not null identity(1,1),
cname char(50),
cstatus char(2),

PRIMARY KEY (cid),

CONSTRAINT Uk_country  UNIQUE (cname)
)
go
------------------------------------------------------State
create table tbl_state
(

sid int not null identity(1,1),
cid int,
statename char(50),
sstatus char(2),

PRIMARY KEY (sid),

CONSTRAINT FK_tbl_country_tbl_state FOREIGN KEY (cid)
    REFERENCES tbl_country(cid),

	CONSTRAINT Uk_statename  UNIQUE (statename)
)
go
-----------------------------------------------------City

create table tbl_city
(
cityid int not null identity(1,1),
sid int ,
cid int,
cityname char(50),
citystatus char(2),

PRIMARY KEY (cityid),

CONSTRAINT FK_tbl_country_tbl_city FOREIGN KEY (cid)
   REFERENCES tbl_country(cid),

CONSTRAINT FK_tbl_state_tbl_city FOREIGN KEY (sid)
   REFERENCES tbl_state(sid),

   CONSTRAINT Uk_City  UNIQUE (cityname)
)
go
----------------------------------------------------------------------------------------------------------City

create table tbl_area
(
areaid int not null identity(1,1),
sid int ,
cid int,
cityid int,
areaname char(50),
areastatus char(2),

PRIMARY KEY (areaid),

CONSTRAINT FK_tbl_country_tbl_area FOREIGN KEY (cid)
   REFERENCES tbl_country(cid),

CONSTRAINT FK_tbl_state_tbl_area FOREIGN KEY (sid)
   REFERENCES tbl_state(sid),

   CONSTRAINT FK_tbl_city_tbl_area FOREIGN KEY (cityid)
     REFERENCES tbl_city(cityid),

   CONSTRAINT Uk_area  UNIQUE (areaname)
)
go
-----------------------------------------------------partytype

create table tbl_partytype
(
partytypeid int not null identity(1,1),
parttype char(50),


PRIMARY KEY (partytypeid),
   CONSTRAINT Uk_tbl_partytype UNIQUE (parttype)
)
go
-------------------------------------------------------------------------
create table tbl_membertype
(
membertypeid int not null identity(1,1),
membertype char(50),


PRIMARY KEY (membertypeid),

   CONSTRAINT Uk_tbl_membertype UNIQUE (membertype)
)

go
-------------------------------------------------------------------------
create table tbl_magzine
(
mid int not null CONSTRAINT pk_tbl_magzine_mid PRIMARY KEY (mid),
mcode nvarchar(50),
mname nvarchar(100),
mtype char(1),
publishname nvarchar(100),
isbn nvarchar(50),
otherdetails nvarchar(100),
mstatus char(1),
cdate date,
CONSTRAINT uk_tbl_magzine_mname UNIQUE (mname),
CONSTRAINT uk_tbl_magzine_mcode UNIQUE (mcode)
)
go

alter table tbl_magzine add constraint uk_tbl_mcode
unique (mcode)
go

-------------------------------------------------------------------------
create table tbl_magzine_price_master
(
priceid int not null identity(1,1),
mid int,
tenure char(50),
rate decimal(15,2),
cdate date,
PRIMARY KEY (priceid),
CONSTRAINT uk_tbl_magzine_price_master UNIQUE (mid,tenure),
)
go
-------------------------------------------------------------------------
create table tbl_magzine_release
(
mrid int not null , -----------magzine Release id
mid int,            -----------magzine ID
mrtitle nvarchar(100), -----------magzine release title
releasecode nvarchar(20),
descrp nvarchar(max) ,
pdf nvarchar(max),
vieourl nvarchar(max),
coverimg nvarchar(max),
thumnail nvarchar(max),
releasedate date,
releasedmonth varchar(10),
releaseyear varchar(4),
ispublish bit,
PRIMARY KEY (mrid),
CONSTRAINT uk_tbl_magzine_release  UNIQUE (mrtitle),
CONSTRAINT fk_tbl_magzine_tbl_magzine_release FOREIGN KEY (mid)REFERENCES tbl_magzine(mid),
)
go

alter table tbl_magzine_release add constraint uk_tbl_releasecode
unique (releasecode)
go
-------------------------------------------------------------------------

create table tbl_magzine_articles
(
arid int not null , -----------article Release id
mid int,            -----------magzine ID
mrid int, -----------magzine Release id
atitlecode nvarchar(100),-----------article code
artitle nvarchar(100), -----------article title
coverimg nvarchar(max),
thumnail nvarchar(max),
descrp nvarchar(max) ,
arurl nvarchar(max) ,
releasedate date,
ispublish bit,
PRIMARY KEY (arid),
CONSTRAINT tbl_magzine_article  UNIQUE (artitle),
CONSTRAINT fk_tbl_magzine_article_tbl_magzine_release FOREIGN KEY (mrid)REFERENCES tbl_magzine_release(mrid),
CONSTRAINT fk_tbl_magzine_article_tbl_magzine FOREIGN KEY (mid)REFERENCES tbl_magzine(mid),
)
go


alter table tbl_magzine_articles add constraint uk_tbl_articlecode
unique (atitlecode)
go
--===============================================================
create table tbl_membership
(
id int identity(1,1),
mscode nvarchar(10)not null, -----------membership code
name nvarchar(100),
mobile nvarchar(10),
WhatsAppNo nvarchar(10),
dob nvarchar(10),
email nvarchar(30),
education nvarchar(max) ,
jobprofile nvarchar(max) ,
gender char(1),
age int,
marital nvarchar(10),
membertypeid int,
isForwarder bit,
parentid nvarchar(10),
IsDeActive bit,
discount decimal(10,2),
constraint pk_tbl_membership PRIMARY KEY(mscode),
CONSTRAINT uk_tbl_membership  UNIQUE (name,mobile),
CONSTRAINT fk_tbl_membership_tbl_membertype FOREIGN KEY (membertypeid)REFERENCES tbl_membertype(membertypeid),
)
go
-----------------------------------------------------------------
alter table tbl_membership add CONSTRAINT uk_tbl_membership_Parentid  UNIQUE (mscode,parentid)
go
-----------------------------------------------------------------
create table tbl_membership_address
(
id int identity(1,1)not null constraint pk_tbl_membership_address PRIMARY KEY,
mscode nvarchar(10)not null, -----------membership code
countryid int,
stateid int,
cityid int,
areaid int,
pincode nvarchar(30),
landkamr nvarchar(max) ,
localaddress nvarchar(max) ,
CONSTRAINT fk_tbl_membership_address_tbl_membertype FOREIGN KEY (mscode)REFERENCES tbl_membership(mscode),
)
go


------------------------------------------------------------------------Subscriptoons

create table tbl_subscriptions_master_detials_subscriber
(
sbid int,
msid int , --membeshipid
mscode nvarchar(10), --membershipcode 
mzid int, -- magzine id
mzname nvarchar(max),--magzine name
mfrequency varchar(100), --magzine freeqquency
mtenure varchar(100), -- magzine tenure
substatus char(1),
fromdate date,
gperiods int,
expdate  date,
expmonth char(15),
expyear char(5),

PRIMARY KEY (sbid),


CONSTRAINT FK_tbl_member_tbl_subscriptions_master_detials_subscriber FOREIGN KEY (mscode)
   REFERENCES tbl_membership(mscode),


CONSTRAINT FK_tbl_magazine_tbl_subscriptions_master_detials_subscriber FOREIGN KEY (mzid)
   REFERENCES tbl_magzine(mid),


CONSTRAINT  UK_tbl_pricemaster UNIQUE  (msid,mzid)

)
go

-------------------------------------------------------------------------

create table tbl_subscriptions
(
sbidlid int not null identity(1,1),
sbid int,
msid int , --membeshipid
mscode nvarchar(10), --membershipcode 
mzid int, -- magzine id
mzname nvarchar(max),--magzine name
mfrequency varchar(100), --magzine freeqquency
mtenure varchar(100), -- magzine tenure

pricemasterid int,

fromdate date,
todate date,
greaseperiods char(3),

rate  decimal(15,2),
amt  decimal(15,2),
gst decimal(15,2),
gstamt decimal(15,2),
dis decimal(15,2),
disamt decimal(15,2),

paybleamt decimal(15,2),
substatus char(1),

PRIMARY KEY (sbidlid),

CONSTRAINT FK_tbl_member_tbl_subscriptions FOREIGN KEY (mscode)
   REFERENCES tbl_membership(mscode),

CONSTRAINT FK_tbl_magazine_tbl_subscriptions FOREIGN KEY (mzid)
   REFERENCES tbl_magzine(mid),

CONSTRAINT FK_tbl_magazine_tbl_pricemaster FOREIGN KEY (pricemasterid)
   REFERENCES tbl_magzine_price_master(priceid),

CONSTRAINT FK_tbl_subscriptions_master_detials_subscriber_tbl_pricemaster FOREIGN KEY (sbid)
   REFERENCES tbl_subscriptions_master_detials_subscriber(sbid),


)
go

-----------------------------------------------------------------------------------------------

create table tbl_master
(
vno int,
vdate date,
vtype char(2),

msid int,
mscode nvarchar(10),


mzid int,
mzname nvarchar(200),
mrid int,
mrname nvarchar(250),

cid int,
sit int,
cityid int,
areaid int,
mtypeid int,
gender char(1),
selecttype char(1),

price decimal(15,0),
rate decimal(15,0),
qty decimal(15,0),
gstper decimal(15,0),
gstamt decimal(15,0),
disper decimal(15,0),
disamt decimal(15,0),
paybleamt decimal(15,0),
totalamt decimal(15,0),


PRIMARY KEY (vno,vtype),


CONSTRAINT FK_tbl_magazine_tbl_master FOREIGN KEY (mzid)
   REFERENCES tbl_magzine(mid),

   
CONSTRAINT FK_tbl_magazinerelease_tbl_master FOREIGN KEY (mrid)
   REFERENCES tbl_magzine_release(mrid),

)
go

-----------------------------------------------

create table tbl_master_ledger
(
lvno int identity(1,1)not null constraint pk_tbl_tbl_master_ledger PRIMARY KEY,
ordeno nvarchar(10),
vno int,
lvdate date,
lvtype char(2),

msid int,
mscode nvarchar(10),

selecttype char(1),


mzid int,
mzname nvarchar(200),

mrid int,
mrname nvarchar(250),

price decimal(15,0),
rate decimal(15,0),
qty decimal(15,0),
gstper decimal(15,0),
gstamt decimal(15,0),
disper decimal(15,0),
disamt decimal(15,0),
paybleamt decimal(15,0),
totalamt decimal(15,0),


 CONSTRAINT FK_tbl_member_tbl_master_ledger FOREIGN KEY (mscode)
   REFERENCES tbl_membership(mscode),

CONSTRAINT FK_tbl_magazine_tbl_master_ledger FOREIGN KEY (mzid)
   REFERENCES tbl_magzine(mid),

   
CONSTRAINT FK_tbl_magazinerelease_tbl_master_ledger FOREIGN KEY (mrid)
   REFERENCES tbl_magzine_release(mrid),


--CONSTRAINT FK_tbl_master_tbl_master_ledger FOREIGN KEY (vno,lvtype)
--    REFERENCES tbl_master(vno,vtype),

)
go

-----------------------------------------------

create table tbl_print
(
pid int identity(1,1)not null constraint pk_tbl_print PRIMARY KEY,
pvno int,
ordeno nvarchar(10),
lvno int,
vno int,
pdate date,
vtype char(2),
mzid  int,
mzname nvarchar(max),
mrid int,
mrname nvarchar(max),
msid int,
localaddress  nvarchar(max),
isfarwarder bit,
isfarwarderid varchar(10),
isfarwarderstatus bit,
addresstosend nvarchar(max),
printqty int,


  --CONSTRAINT tbl_master_tbl_prints FOREIGN KEY (vno,vtype)
  -- REFERENCES tbl_master(vno,vtype),

     CONSTRAINT tbl_master_ledger_tbl_print FOREIGN KEY (lvno)
   REFERENCES tbl_master_ledger(lvno),

)
go

-----------------------------------------------

create table tbl_dispatch
(
did int identity(1,1)not null constraint pk_tbl_dispatch PRIMARY KEY,
dvno int,
ordeno nvarchar(10),
ddate date,
vno int,
lvno int,
pvno int,
pid  int,
vtype char(2),
mzid int,
mzname nvarchar(200),
mrid int,
mrname nvarchar(250),
msid int,
dispatchmod nvarchar(max),
dispatchremarks nvarchar(max),
deldate date,
dstatusremakrs nvarchar(max),
dispatchstatus char(1),



   CONSTRAINT tbl_master_print_tbl_dispatch FOREIGN KEY (pid)
   REFERENCES tbl_print(pid),


     CONSTRAINT tbl_master_ledger_tbl_dispatch FOREIGN KEY (lvno)
   REFERENCES tbl_master_ledger(lvno),


)
go
-------------------------------------------------------------------------





