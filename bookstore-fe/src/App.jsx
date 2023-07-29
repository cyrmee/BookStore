import React, { useState, useEffect } from 'react';
import './App.css'
import Book from './Components/Book';

function App() {
  return (
    <div className='bg-black'>
      <Book color={"bg-green-900"}/>
      <Book color={"bg-yellow-900"}/>
      <Book color={"bg-red-900"}/>
    </div>
  );
}

export default App
