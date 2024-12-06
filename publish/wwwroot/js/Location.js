document.getElementById('synopsis-tab').addEventListener('click', () => {
    document.getElementById('content').innerHTML = `
      <p>
          Wicked tells the untold story of Elphaba, a misunderstood young woman with green skin, and Glinda, a privileged and ambitious student, 
          as they meet at Shiz University in Oz. Their unlikely friendship is tested after an encounter with the Wizard of Oz, leading them on different 
          paths—Glinda seeking power and popularity, and Elphaba staying true to herself, with life-changing consequences. Ultimately, their journeys shape 
          them into Glinda the Good and the Wicked Witch of the West.
      </p>
    `;
    setActiveTab('synopsis-tab');
  });

  document.getElementById('cast-tab').addEventListener('click', () => {
    document.getElementById('content').innerHTML = `
      <p>
        <strong>Director:</strong> <br>
        Jonathan Murray Chu<br><br>
        <strong>Cast</strong> <br>
        Cynthia Erivo, Ariana Grande, Michelle Yeoh, Jeff Goldblum, Jonathan Bailey, Ethan Slater<br><br>
        <strong>Genre</strong><br>Fantasy<br><br> 
      </p>
    `;
    setActiveTab('cast-tab');
  });

  function setActiveTab(tabId) {
    document.querySelectorAll('.tab').forEach(tab => {
      tab.classList.remove('active');
    });
    document.getElementById(tabId).classList.add('active');
  }

  function openTrailer() {
      const youtubeTrailerURL = 'https://youtu.be/6COmYeLsz4c?si=2lJP3AQMr0BcdfF5';
      window.open(youtubeTrailerURL, '_blank'); //buka di tab baru
  }