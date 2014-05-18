<?php
	if(!isset($_GET['page']))
		include("home.php");
	else if ($_GET['page']=='home')
		include("home.php");
	else if ($_GET['page']=='portfolio')
		include("portfolio.php");
	else if ($_GET['page']=='projects')
		include("projects.php");
	else if ($_GET['page']=='contact')
		include("contact.php");
?>