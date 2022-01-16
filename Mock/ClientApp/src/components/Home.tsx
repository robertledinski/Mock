import * as React from 'react';
import { connect } from 'react-redux';

const Home = () => (
  <div>
    <h1>Hello, Mock!</h1>
    <p>This application is Code test.</p>
    <ul>
      <li>In right upper sidebar you can find Navbar link to Account Data which shows parsed Account.json, saved to DB via seed script and presented using react.</li>
    </ul>
    <ul>
            <li>The second part of task was to create API where one can provide an object similar to Account.json and response would be calculated balance grouped by days.</li>
            <li>It can be tested by sending JSON object to endpoint Account/MockAPI via POST method.</li>
    </ul>
  </div>
);

export default connect()(Home);
