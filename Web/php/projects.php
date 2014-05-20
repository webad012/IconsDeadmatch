<ul>
	<li><a href="index.php?page=projects&game=iconsdeathmatch">Icons Deathmatch</a></li>
	<?php
		if(isset($_GET['game']) && $_GET['game']=='iconsdeathmatch')
		{
			echo '<a href="http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/IconsDeathmatchWin_0_4.zip">Download: v0.4 Windows32b 35MB</a>'
					.'</br>'
					.'<a href="http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/IconsDeathmatchLin_0_4.zip">Download: v0.4 Linux32b 37MB</a>';
			echo '</br></br>Note: on 64b linux install "libglu1-mesa:i386" and "libxcursor1:i386"';
		}
	?>
</ul>