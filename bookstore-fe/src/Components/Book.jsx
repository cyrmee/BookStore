import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Book = ({ color }) => {
    const [books, setBooks] = useState([]);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const response = await axios.get('https://localhost:7093/api/Book');
            setBooks(response.data);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const handleAdd = (e) => {
        e.preventDefault()
        localStorage.setItem('token', '12345')
    }

    const handleRemove = (e) => {
        e.preventDefault()
        localStorage.removeItem('token')
    }

    return (
        <div className={`${color} flex justify-center items-center py-24 h-screen`}>
            <table className='text-white text-center '>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Author</th>
                        <th>Publisher</th>
                        <th>Publication Date</th>
                        <th>ISBN-10</th>
                        <th>ISBN-13</th>
                        <th>Quantity</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map((book) => (
                        <tr key={book.id}>
                            <td className='px-2'>{book.title}</td>
                            <td className='px-2'>{book.author}</td>
                            <td className='px-2'>{book.publisher}</td>
                            <td className='px-2'>{book.publicationDate}</td>
                            <td className='px-2'>{book.isbn10}</td>
                            <td className='px-2'>{book.isbn13}</td>
                            <td className='px-2'>{book.quantity}</td>
                            <td className='px-2'>{book.price}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div className='flex flex-col gap-2'>
                <button onClick={handleAdd} className='bg-white px-2 py-1 rounded'>Add Token</button>
                <button onClick={handleRemove} className='bg-white px-2 py-1 rounded'>Remove Token</button>
            </div>
        </div>
    )
}

export default Book