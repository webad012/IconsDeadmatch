<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
	<meta name="description" content="milos jankovic webad"/>
	<meta name="keywords" content="milos, jankovic, webad, javascript"/> 
	<meta name="author" content="Milos Jankovic"/> 
	<script type="text/javascript" src="sat.js"></script>
	<link rel="stylesheet" type="text/css" href="css/milosjankovic.css" media="screen"/>
	<script type="text/javascript" src="bookmark.js"></script>
	<script type="text/javascript" src="sat.js"></script>

	<title>Miloš Janković</title>
</head>

<body onload="startTime()">
	<div class="outer-container">
		<div class="inner-container">
			<div class="header">
				<?php
					include("php/header.php");
				?>
			</div>
			<div class="path">
				<div align="left">
					<?php
						if(!isset($_GET['page']))
							echo 'Home &#8250;<a href="index.php?page=home"> Početna strana</a>';
						else if ($_GET['page']=='home')
							echo 'Home &#8250;<a href="index.php?page=home"> Početna strana</a>';
						else if ($_GET['page']=='portfolio')
							echo 'Home &#8250;<a href="index.php?page=portfolio"> Portfolio</a>';
						else if ($_GET['page']=='projects')
							echo 'Home &#8250;<a href="index.php?page=projects"> Projekti</a>';
						else if ($_GET['page']=='contact')
							echo 'Home &#8250;<a href="index.php?page=contact"> Kontakt</a>';
					?>
				</div>
			</div>
			<div class="main">		
				<div class="content">
					<?php
						include("php/content.php");
					?>
				</div>
				<div class="navigation">
					<?php
						include("php/navigation.php");
					?>
				</div>
				<div class="clearer"></div>
			</div>
			<div class="footer">
				<p align="center"><span class="left"> &copy; 2008</span><span class="right">by Miloš Janković</span></p>
			</div>
		</div>
	</div>
</body>
</html>
