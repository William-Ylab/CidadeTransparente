/*
SQLyog Community v11.31 (64 bit)
MySQL - 5.1.49-community : Database - fdt
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`fdt` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `fdt`;

/*Table structure for table `answer` */

DROP TABLE IF EXISTS `answer`;

CREATE TABLE `answer` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BaseQuestionId` bigint(20) NOT NULL DEFAULT '0',
  `ResponseFormId` bigint(20) NOT NULL DEFAULT '0',
  `Score` decimal(10,2) DEFAULT NULL,
  `Observation` mediumtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Answer_ResponseForm` (`ResponseFormId`),
  KEY `FK_Answer_BaseQuestion` (`BaseQuestionId`),
  CONSTRAINT `FK_Answer_BaseQuestion` FOREIGN KEY (`BaseQuestionId`) REFERENCES `basequestion` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Answer_ResponseForm` FOREIGN KEY (`ResponseFormId`) REFERENCES `responseform` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `baseblock` */

DROP TABLE IF EXISTS `baseblock`;

CREATE TABLE `baseblock` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BaseFormId` bigint(20) DEFAULT NULL,
  `Name` varchar(150) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `FK_BaseBlock_BaseForm` (`BaseFormId`),
  CONSTRAINT `FK_BaseBlock_BaseForm` FOREIGN KEY (`BaseFormId`) REFERENCES `baseform` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=latin1;

/*Table structure for table `baseform` */

DROP TABLE IF EXISTS `baseform`;

CREATE TABLE `baseform` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PeriodId` bigint(20) NOT NULL DEFAULT '0',
  `Name` varchar(100) NOT NULL DEFAULT '',
  `CreationDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`),
  KEY `FK_BaseForm_Period` (`PeriodId`),
  CONSTRAINT `FK_BaseForm_Period` FOREIGN KEY (`PeriodId`) REFERENCES `period` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

/*Table structure for table `basequestion` */

DROP TABLE IF EXISTS `basequestion`;

CREATE TABLE `basequestion` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BaseSubBlockId` bigint(20) NOT NULL DEFAULT '0',
  `Question` mediumtext NOT NULL,
  `Value` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Index` int(3) NOT NULL DEFAULT '0',
  `Tip` mediumtext,
  PRIMARY KEY (`Id`),
  KEY `FK_BaseBlock` (`BaseSubBlockId`),
  CONSTRAINT `FK_BaseQuestion_BaseSubBlock` FOREIGN KEY (`BaseSubBlockId`) REFERENCES `basesubblock` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `basesubblock` */

DROP TABLE IF EXISTS `basesubblock`;

CREATE TABLE `basesubblock` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BaseBlockId` bigint(20) NOT NULL DEFAULT '0',
  `Name` varchar(100) NOT NULL DEFAULT '',
  `Weight` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Index` int(3) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK_BaseSubBlock_BaseBlock` (`BaseBlockId`),
  CONSTRAINT `FK_BaseSubBlock_BaseBlock` FOREIGN KEY (`BaseBlockId`) REFERENCES `baseblock` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8;

/*Table structure for table `city` */

DROP TABLE IF EXISTS `city`;

CREATE TABLE `city` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `StateId` varchar(2) NOT NULL DEFAULT '',
  `Name` varchar(200) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `FK_City_State` (`StateId`),
  CONSTRAINT `FK_City_State` FOREIGN KEY (`StateId`) REFERENCES `state` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5565 DEFAULT CHARSET=utf8;

/*Table structure for table `group` */

DROP TABLE IF EXISTS `group`;

CREATE TABLE `group` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CityId` bigint(20) NOT NULL DEFAULT '0',
  `ResponsableId` bigint(20) DEFAULT NULL,
  `PeriodId` bigint(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `CityId_ResponsableId_PeriodId` (`CityId`,`ResponsableId`,`PeriodId`),
  KEY `FK_Group_City` (`CityId`),
  KEY `FK_Group_Responsable` (`ResponsableId`),
  KEY `FK_Group_Period` (`PeriodId`),
  CONSTRAINT `FK_Group_City` FOREIGN KEY (`CityId`) REFERENCES `city` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Group_Period` FOREIGN KEY (`PeriodId`) REFERENCES `period` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Group_Responsable` FOREIGN KEY (`ResponsableId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `group_user` */

DROP TABLE IF EXISTS `group_user`;

CREATE TABLE `group_user` (
  `GroupId` bigint(20) NOT NULL,
  `UserId` bigint(20) NOT NULL,
  PRIMARY KEY (`GroupId`,`UserId`),
  KEY `FK_GroupUser_Group` (`GroupId`),
  KEY `FK_GroupUser_User` (`UserId`),
  CONSTRAINT `FK_GroupUser_Group` FOREIGN KEY (`GroupId`) REFERENCES `group` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_GroupUser_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `log_201312` */

DROP TABLE IF EXISTS `log_201312`;

CREATE TABLE `log_201312` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=42 DEFAULT CHARSET=latin1;

/*Table structure for table `log_201401` */

DROP TABLE IF EXISTS `log_201401`;

CREATE TABLE `log_201401` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=160 DEFAULT CHARSET=latin1;

/*Table structure for table `log_201402` */

DROP TABLE IF EXISTS `log_201402`;

CREATE TABLE `log_201402` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=66 DEFAULT CHARSET=latin1;

/*Table structure for table `log_201403` */

DROP TABLE IF EXISTS `log_201403`;

CREATE TABLE `log_201403` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

/*Table structure for table `log_201404` */

DROP TABLE IF EXISTS `log_201404`;

CREATE TABLE `log_201404` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `log_201405` */

DROP TABLE IF EXISTS `log_201405`;

CREATE TABLE `log_201405` (
  `Id` bigint(18) NOT NULL AUTO_INCREMENT,
  `Guid` varchar(255) NOT NULL DEFAULT '',
  `ApplicationName` varchar(50) NOT NULL DEFAULT '',
  `Path` varchar(255) NOT NULL DEFAULT '',
  `Content` longtext NOT NULL,
  `Level` tinyint(4) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

/*Table structure for table `period` */

DROP TABLE IF EXISTS `period`;

CREATE TABLE `period` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL DEFAULT '',
  `InitialDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  `FinalDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  `ConvocationInitialDate` datetime DEFAULT '1901-01-01 00:00:00',
  `ConvocationFinalDate` datetime DEFAULT '1901-01-01 00:00:00',
  `Open` tinyint(1) NOT NULL DEFAULT '0',
  `Published` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

/*Table structure for table `request_city` */

DROP TABLE IF EXISTS `request_city`;

CREATE TABLE `request_city` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CityId` bigint(20) NOT NULL DEFAULT '0',
  `UserId` bigint(20) NOT NULL DEFAULT '0',
  `PeriodId` bigint(20) NOT NULL DEFAULT '0',
  `RequestType` int(4) NOT NULL DEFAULT '0',
  `RequestDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  `Status` int(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `CityId_UserId_PeriodId` (`CityId`,`UserId`,`PeriodId`),
  KEY `FK_RQ_UserId` (`UserId`),
  KEY `FK_RQ_PeriodId` (`PeriodId`),
  CONSTRAINT `FK_RQ_CityId` FOREIGN KEY (`CityId`) REFERENCES `city` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_RQ_PeriodId` FOREIGN KEY (`PeriodId`) REFERENCES `period` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_RQ_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `responseform` */

DROP TABLE IF EXISTS `responseform`;

CREATE TABLE `responseform` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `BaseFormId` bigint(20) NOT NULL DEFAULT '0',
  `CityId` bigint(20) DEFAULT NULL,
  `UserId` bigint(20) NOT NULL DEFAULT '0',
  `TrackingNote` mediumtext NOT NULL,
  `TotalScore` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`),
  KEY `FK_ResponseForm_BaseForm` (`BaseFormId`),
  KEY `FK_ResponseForm_City` (`CityId`),
  KEY `FK_ResponseForm_User` (`UserId`),
  CONSTRAINT `FK_ResponseForm_BaseForm` FOREIGN KEY (`BaseFormId`) REFERENCES `baseform` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_ResponseForm_City` FOREIGN KEY (`CityId`) REFERENCES `city` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_ResponseForm_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `review` */

DROP TABLE IF EXISTS `review`;

CREATE TABLE `review` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) NOT NULL DEFAULT '0',
  `ResponseFormId` bigint(20) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  `Accepted` tinyint(1) NOT NULL DEFAULT '0',
  `Observations` mediumtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Review_User` (`UserId`),
  KEY `FK_Review_ResponseForm` (`ResponseFormId`),
  CONSTRAINT `FK_Review_ResponseForm` FOREIGN KEY (`ResponseFormId`) REFERENCES `responseform` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Review_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `state` */

DROP TABLE IF EXISTS `state`;

CREATE TABLE `state` (
  `Id` varchar(2) NOT NULL,
  `Name` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `submit` */

DROP TABLE IF EXISTS `submit`;

CREATE TABLE `submit` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ResponseFormId` bigint(20) NOT NULL DEFAULT '0',
  `Date` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  `Observation` mediumtext NOT NULL,
  `Status` int(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `FK_Submit_ResponseForm` (`ResponseFormId`),
  CONSTRAINT `FK_Submit_ResponseForm` FOREIGN KEY (`ResponseFormId`) REFERENCES `responseform` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CNPJ` varchar(20) NOT NULL DEFAULT '',
  `Name` varchar(50) NOT NULL DEFAULT '',
  `Organization` varchar(150) NOT NULL DEFAULT '',
  `ContactName` varchar(100) NOT NULL DEFAULT '',
  `ContactEmail` varchar(50) NOT NULL DEFAULT '',
  `Nature` varchar(100) NOT NULL DEFAULT '',
  `Area` varchar(100) NOT NULL DEFAULT '',
  `Range` varchar(100) NOT NULL DEFAULT '',
  `Address` varchar(150) NOT NULL DEFAULT '',
  `Number` varchar(50) NOT NULL DEFAULT '',
  `Neighborhood` varchar(100) NOT NULL DEFAULT '',
  `Region` varchar(20) NOT NULL DEFAULT '',
  `CityId` bigint(20) DEFAULT NULL,
  `ZipCode` varchar(10) NOT NULL DEFAULT '',
  `Phone` varchar(30) NOT NULL DEFAULT '',
  `WebSite` varchar(150) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL DEFAULT '',
  `Login` varchar(20) NOT NULL DEFAULT '',
  `Password` varchar(255) NOT NULL DEFAULT '',
  `UserType` int(4) NOT NULL DEFAULT '0',
  `Active` tinyint(1) NOT NULL DEFAULT '1',
  `Network` tinyint(1) NOT NULL DEFAULT '0',
  `Thumb` blob,
  `Mime` varchar(100) DEFAULT NULL,
  `NetworkApproved` tinyint(1) NOT NULL DEFAULT '0',
  `NetworkApprovedById` bigint(20) DEFAULT NULL,
  `TermsOfUse` tinyint(1) NOT NULL DEFAULT '0',
  `AcceptionTermsDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
  PRIMARY KEY (`Id`),
  KEY `FK_User_City` (`CityId`),
  KEY `FK_User_User` (`NetworkApprovedById`),
  CONSTRAINT `FK_User_City` FOREIGN KEY (`CityId`) REFERENCES `city` (`Id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `FK_User_User` FOREIGN KEY (`NetworkApprovedById`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
