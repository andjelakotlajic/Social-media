﻿using Microsoft.AspNetCore.Http.HttpResults;
using TeamApacheProjekatBackend.Dto;
using TeamApacheProjekatBackend.Models;
using TeamApacheProjekatBackend.Repositories.Interfaces;
using TeamApacheProjekatBackend.Services.Interfaces;

namespace TeamApacheProjekatBackend.Services
{
    public class CommentService : ICommentService
    {
        public readonly ICommentRepository _commentRepository;
        //public readonly IMapper _mapper;
        public CommentService(ICommentRepository commentRepository/*, IMapper mapper*/)
        {
            _commentRepository = commentRepository;
            //_mapper = mapper;
        }

        public async Task<CommentGetDetailsResponseDTO> GetAsync(int id)
        {
            var comment  = await _commentRepository.FindById( id );
            var  returnDTO = new CommentGetDetailsResponseDTO()
            {
                Id = comment.Id,
                PostId = comment.Post.Id,
                UserId = comment.User.Id
            };
            return returnDTO;
        }

        public async Task<IEnumerable<CommentGetDetailsResponseDTO>> GetAllAsync(int postId)
        {
            var comments = await _commentRepository.FindAll(postId);
            //return _mapper.Map<IEnumerable<CommentGetDetailsResponseDTO>>(comments);
            List<CommentGetDetailsResponseDTO> commentsList = new List<CommentGetDetailsResponseDTO>();
            foreach (var comment in comments)
            {
                var dto = new CommentGetDetailsResponseDTO()
                {
                    Id = comment.Id, PostId = comment.Post.Id, Text = comment.Text, UserId = comment.User.Id
                };
                commentsList.Add(dto);
            }
            return commentsList;
        }

        public async Task<CommentGetDetailsResponseDTO> CreateAsync(CommentCreateRequestDTO commentDTO)
        {
            //var commentEntity = _mapper.Map<Comment>(commentDTO);
            var commentEntity = new Comment { PostId = commentDTO.PostId, Text=commentDTO.Text, UserId = commentDTO.UserId };
            var result = await _commentRepository.InsertComment(commentEntity);
            var returnDTO = new CommentGetDetailsResponseDTO { Id = result.Id, UserId = result.User.Id, PostId = result.Post.Id, Text = result.Text };
            //return _mapper.Map<CommentGetDetailsResponseDTO>(result);
            return returnDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _commentRepository.FindById(id);
            if (exists == null)
            {
                return false;
            }

            _commentRepository.DeleteComment(exists.Id);

            return true;
        }

        public void Update(CommentCreateRequestDTO commentDTO)
        {
            var commentEntity = new Comment { PostId = commentDTO.PostId, Text = commentDTO.Text, UserId = commentDTO.UserId };
            _commentRepository.UpdateComment(commentEntity);
        }
    }
}