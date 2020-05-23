create database tweety
use tweety

create table [user](
[userID] int primary key,
[name] varchar(30) UNIQUE NOT NULL,
[password] varchar(30) NOT NULL,
[displayPic] varchar(1000)
)

create table [profile](
userID int foreign key references [user](userID) on delete cascade on update cascade not null,
fname varchar(30) NOT NULL,
lname varchar(30) ,
gender char check (gender = 'F' OR gender = 'M'),
DOB date ,
email varchar(30),
country varchar(30),
status varchar(120)
)

create table tweets(
tweetID int primary key,
userID int foreign key references [user](userID) on delete cascade on update cascade,
tweet varchar(280) NOT NULL,
--[time] timestamp NOT NULL
[date] date NOT NULL,
[time] time NOT NULL
)

create table hashtag(
hashtagID int,
tweetID int foreign key references [tweets](tweetID) on delete cascade on update cascade,
hashtag varchar(50) NOT NULL,
primary key(hashtagID , tweetID)
)

create table follower(
[userID] int foreign key references [user](userID) on delete cascade on update cascade,
followerID int foreign key references [user](userID),
primary key([userID],followerID)
)

create table likes(
likerID int foreign key references [user](userID) on delete no action on update no action,
tweetID int foreign key references [tweets](tweetID) on delete no action on update no action,
primary key(likerID,tweetID)
)

create table dislikes(
dislikerID int foreign key references [user](userID) on delete no action on update no action,
tweetID int foreign key references [tweets](tweetID) on delete no action on update no action,
primary key(dislikerID,tweetID)
)

create table comments(
commentID int primary key,
tweetID int NOT NULL foreign key references [tweets](tweetID) on delete cascade on update cascade,
commenterID int NOT NULL foreign key references [user](userID) on delete no action on update no action,
comment varchar(280) NOT NULL,
--[time] timestamp 
[date] date NOT NULL,
[time] time NOT NULL
)

create table privateChat(
chatID int ,
senderID int foreign key references [user](userID) on delete no action on update no action,
receiverID int foreign key references [user](userID)on delete no action on update no action,
messageID int ,
[message] varchar(280) NOT NULL,
--[time] timestamp,
[date] date NOT NULL,
[time] time NOT NULL,
primary key(chatID,messageID)
)


create table Notifications(
notificationID int primary key,
userID int not null foreign key references [user](userID) on delete no action on update no action,
nDate datetime not null,
nTime time not null,
readFlag varchar(1) not null,
n_Text varchar(200) not null
)

select *
from Notifications

select *
from likes

select *
from dislikes
