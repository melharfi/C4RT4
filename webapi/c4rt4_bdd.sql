-- phpMyAdmin SQL Dump
-- version 3.3.2
-- http://www.phpmyadmin.net
--
-- Serveur: 127.0.0.1
-- Généré le : Sam 14 Août 2010 à 02:22
-- Version du serveur: 5.1.45
-- Version de PHP: 5.3.2

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données: `c4rt4_bdd`
--

-- --------------------------------------------------------

--
-- Structure de la table `parties_app_zriwita`
--

CREATE TABLE IF NOT EXISTS `parties_app_zriwita` (
  `id` int(100) NOT NULL AUTO_INCREMENT,
  `nom_table` varchar(100) NOT NULL,
  `owner` varchar(100) NOT NULL,
  `evalue` int(100) NOT NULL,
  `nbr_players` int(100) NOT NULL,
  `ip_host` varchar(100) NOT NULL,
  `clients` text NOT NULL,
  `locked` int(10) NOT NULL DEFAULT '0',
  `etat` varchar(100) NOT NULL DEFAULT 'En attente',
  `banned` text NOT NULL,
  `extra` varchar(100) NOT NULL DEFAULT '7-6-10-11',
  `chat` text NOT NULL,
  `chat_cnt` int(11) NOT NULL DEFAULT '0',
  KEY `id` (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=41 ;

--
-- Contenu de la table `parties_app_zriwita`
--


-- --------------------------------------------------------

--
-- Structure de la table `signed_app`
--

CREATE TABLE IF NOT EXISTS `signed_app` (
  `id` int(100) NOT NULL AUTO_INCREMENT,
  `id_app` varchar(100) NOT NULL,
  `real_id` int(100) NOT NULL,
  `user` varchar(100) NOT NULL,
  `ip` varchar(100) NOT NULL,
  `time` int(100) NOT NULL,
  UNIQUE KEY `id_user` (`id_app`,`user`),
  KEY `id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='session des app C4RT4' AUTO_INCREMENT=1 ;

--
-- Contenu de la table `signed_app`
--


-- --------------------------------------------------------

--
-- Structure de la table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(100) NOT NULL AUTO_INCREMENT,
  `nom` varchar(100) NOT NULL,
  `user` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `ville` varchar(100) NOT NULL,
  `avatar_id` int(100) NOT NULL,
  `points` int(100) NOT NULL,
  `nbr_parties` varchar(100) NOT NULL DEFAULT '0/0',
  `subscribed` varchar(100) NOT NULL DEFAULT '',
  `communaute_name` varchar(100) NOT NULL DEFAULT 'null',
  PRIMARY KEY (`id`),
  UNIQUE KEY `user` (`user`,`avatar_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 COMMENT='table des utilisateurs' AUTO_INCREMENT=3 ;

--
-- Contenu de la table `users`
--

INSERT INTO `users` (`id`, `nom`, `user`, `password`, `ville`, `avatar_id`, `points`, `nbr_parties`, `subscribed`, `communaute_name`) VALUES
(1, 'mohssine', 'mor', 'e10adc3949ba59abbe56e057f20f883e', 'rabat', 0, 150, '0/0', '01/08/10 02:23:21', 'null'),
(2, 'hamid', 'pop', 'e10adc3949ba59abbe56e057f20f883e', 'rabat', 0, 250, '0/0', '03/08/10 10:49:00', 'null');
